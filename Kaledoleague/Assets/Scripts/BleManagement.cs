using System ;
using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;
using UnityEngine.UI ;
using System.Linq ;
using Bluetooth.Helpers ;
using UnityEditor;
using UnityEngine.Android ;
using UnityEngine.Events ;

[System.Serializable]
public class HeartRateEvent : UnityEvent<double> { }

public enum Devices { EEG, HRM } ;

public class BleManagement : MonoBehaviour
{

    public HeartRateEvent heartRateEvent ;

    public string MAC_ADDRESS = "" ;
    public Devices device ;

    private double [][] lastEEGData ;
    private double lastHRMData ;

    private AndroidJavaObject BluetoothManager, BluetoothAdapter, BluetoothDevice, BluetoothGatt ;
    private AndroidJavaObject bluetoothAdapter, bluetoothGatt, gattService, gattCharacteristic ;

    bool isReady = false ;

    readonly  string descriptorID               = "00002902-0000-1000-8000-00805f9b34fb" ;

    readonly  string serviceHRMID               = "0000180D-0000-1000-8000-00805f9b34fb" ;
    readonly  string characteristicHRM          = "00002A37-0000-1000-8000-00805f9b34fb" ;

    readonly string serviceCustomID             = "0000fe8d-0000-1000-8000-00805f9b34fb" ;
    readonly string characteristicTP9           = "273e0003-4c4d-454d-96be-f03bac821358" ;
    readonly string characteristicAF7           = "273e0004-4c4d-454d-96be-f03bac821358" ;
    readonly string characteristicAF8           = "273e0005-4c4d-454d-96be-f03bac821358" ;
    readonly string characteristicTP10          = "273e0006-4c4d-454d-96be-f03bac821358" ;
    readonly string characteristicStreamToggle  = "273e0001-4c4d-454d-96be-f03bac821358" ;

    byte[] value_ask = { 0x02, 0x73, 0x0A } ;
    byte[] value_stream = { 0x02, 0x64, 0x0A } ;

    // Use this for initialization
    void Start ()
    {
        lastEEGData = new double [4][] ;
        for (int i = 0 ; i < 4 ; i++)
            lastEEGData[i] = new double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } ;
        lastHRMData = 0 ;
    }

    public double[][] GetEEGData()
    {
        return lastEEGData ;
    }

    public double GetHRMData()
    {
        return lastHRMData ;
    }
 
    IEnumerator InitEEG()
    {
        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            BluetoothManager = activity.Call<AndroidJavaObject>("getSystemService", "bluetooth") ;
            BluetoothAdapter = BluetoothManager.Call<AndroidJavaObject>("getAdapter") ;
            BluetoothAdapter.Call<bool>("enable") ;
            BluetoothDevice = BluetoothAdapter.Call<AndroidJavaObject>("getRemoteDevice", MAC_ADDRESS) ;   

            AndroidJavaObject myBluetoothGattCallback = new AndroidJavaObject("MyBluetoothGattCallback");
            MyBluetoothGattCallback bluetoothGattCallback = new MyBluetoothGattCallback() ;    

            bluetoothGattCallback.onCharacteristicChangedDelegate = (g, c) =>
            {
                int iid = c.Call<int>("getInstanceId") ;
                var data = c.Call<byte[]>("getValue") ;
                   
                string hexStr = BitConverter.ToString(data) ;
                string[] hexSplit = hexStr.Split('-') ;
                string bits = string.Empty ;
                foreach (var hex in hexSplit)
                {
                    ushort longValue = Convert.ToUInt16("0x" + hex, 16);
                    bits = bits + Convert.ToString(longValue, 2).PadLeft(8, '0');
                }

                double[] samples = new double[12] ;
                for (int i = 0; i < 12; i++)
                {
                    samples[i] = PacketConversion.ToUInt12(bits, 16 + (i * 12)); // Initial offset by 16 bits for the timestamp.
                    samples[i] = (samples[i] - 2048d) * 0.48828125d; // 12 bits on a 2 mVpp range.
                }

                switch (iid)
                {
                    case 32 :
                        lastEEGData[0] = samples ;
                        break ;
                    case 35 :
                        lastEEGData[1] = samples ;
                        break ;
                    case 38 :
                        lastEEGData[2] = samples ;
                        break ;
                    case 41 :
                        lastEEGData[3] = samples ;
                        break ;
                    default :
                        break ;
                }
                Debug.Log("BLUETOOTH CHAR " + iid + " with VALUE " + string.Join(", ", samples)) ;                    
            } ;

            bluetoothGattCallback.onCharacteristicWriteDelegate = (g, c, s) =>
            {
                Debug.Log("BLUETOOTH STREAM CHARACTERISTIC WRITE CALLBACK " + s) ;
            } ;

            bluetoothGattCallback.onDescriptorWriteDelegate = (g, d, s) =>
            {
                Debug.Log("BLUETOOTH NOTIFICATION DESCRIPTOR WRITE CALLBACK " + s) ;
            } ;

            bluetoothGattCallback.onConnectionStateChangeDelegate = (g, s, n) =>
            {
                switch (n)
                {
                    case 0 :
                        Debug.Log("BLUETOOTH CSCD 0") ;
                        break ;
                    case 1 :
                        Debug.Log("BLUETOOTH CSCD 1") ;
                        break ;
                    case 2 :
                        Debug.Log("BLUETOOTH CSCD 2"+g.Call<AndroidJavaObject>("getDevice").Call<string>("getName")) ;
                        var isFind= g.Call<bool>("discoverServices") ;
                        break ;
                    case 3:
                        Debug.Log("BLUETOOTH CSCD 3") ;
                        break ;
                    default:
                        break ;
                }
            } ;
        
            bluetoothGattCallback.onServicesDiscoveredDelegate = (g, c) =>
            {
                var gattServices = g.Call<AndroidJavaObject>("getServices") ;
                Debug.Log("BLUETOOTH SDD..." + gattServices.Get<int>("size")) ;
                isReady = true ;
            } ;

            myBluetoothGattCallback.Call("AddUnityCallback", bluetoothGattCallback) ;
            BluetoothGatt = BluetoothDevice.Call<AndroidJavaObject>("connectGatt", activity, true, myBluetoothGattCallback) ;

            while (!isReady) ;

            Debug.Log("BLUETOOTH BEGIN SET UP") ;

            gattCharacteristic = GetCharacteristic(GetService(BluetoothGatt, serviceCustomID), characteristicStreamToggle) ;
            gattCharacteristic.Call("setWriteType", 1) ;
            bool b = gattCharacteristic.Call<bool>("setValue", value_ask) ;  
            b = BluetoothGatt.Call<bool>("writeCharacteristic", gattCharacteristic) ;                   
            yield return new WaitForSeconds(1) ; 
            b = gattCharacteristic.Call<bool>("setValue", value_stream) ;
            b = BluetoothGatt.Call<bool>("writeCharacteristic", gattCharacteristic) ;                                          
            gattCharacteristic.Call("setWriteType", 2) ;
                
            yield return new WaitForSeconds(1) ; 
            gattCharacteristic = GetCharacteristic(GetService(BluetoothGatt, serviceCustomID), characteristicTP9) ;
            NotificationData(BluetoothGatt, gattCharacteristic, descriptorID) ;

            yield return new WaitForSeconds(1) ; 
            gattCharacteristic = GetCharacteristic(GetService(BluetoothGatt, serviceCustomID), characteristicAF7) ;
            NotificationData(BluetoothGatt, gattCharacteristic, descriptorID) ;
                
            yield return new WaitForSeconds(1) ;  
            gattCharacteristic = GetCharacteristic(GetService(BluetoothGatt, serviceCustomID), characteristicAF8) ;
            NotificationData(BluetoothGatt, gattCharacteristic, descriptorID) ;

            yield return new WaitForSeconds(1) ; 
            gattCharacteristic = GetCharacteristic(GetService(BluetoothGatt, serviceCustomID), characteristicTP10) ;
            NotificationData(BluetoothGatt, gattCharacteristic, descriptorID) ;          
        }
    }
    

    public void InitHRM()
    {
        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            BluetoothManager = activity.Call<AndroidJavaObject>("getSystemService", "bluetooth") ;
            BluetoothAdapter = BluetoothManager.Call<AndroidJavaObject>("getAdapter") ;
            BluetoothAdapter.Call<bool>("enable") ;
            BluetoothDevice = BluetoothAdapter.Call<AndroidJavaObject>("getRemoteDevice", MAC_ADDRESS) ;   

            AndroidJavaObject myBluetoothGattCallback = new AndroidJavaObject("MyBluetoothGattCallback");
            MyBluetoothGattCallback bluetoothGattCallback = new MyBluetoothGattCallback() ;    

            bluetoothGattCallback.onCharacteristicChangedDelegate = (g, c) =>
            {
                int iid = c.Call<int>("getInstanceId") ;
                var data = c.Call<byte[]>("getValue") ;
                   
                switch (iid)
                {
                    case 13 :
                        lastHRMData = data[1] ;
						heartRateEvent.Invoke(data[1]) ;
                        break ;
                    default :
                        break ;
                }
                    
                Debug.Log("BLUETOOTH CHAR " + iid + " with VALUE " + data[1]) ;                    
            } ;

            bluetoothGattCallback.onCharacteristicWriteDelegate = (g, c, s) =>
            {
                Debug.Log("BLUETOOTH STREAM CHARACTERISTIC WRITE CALLBACK " + s) ;
            } ;

            bluetoothGattCallback.onDescriptorWriteDelegate = (g, d, s) =>
            {
                Debug.Log("BLUETOOTH NOTIFICATION DESCRIPTOR WRITE CALLBACK " + s) ;
            } ;

            bluetoothGattCallback.onConnectionStateChangeDelegate = (g, s, n) =>
            {
                switch (n)
                {
                    case 0 :
                        Debug.Log("BLUETOOTH CSCD 0") ;
                        break ;
                    case 1 :
                        Debug.Log("BLUETOOTH CSCD 1") ;
                        break ;
                    case 2 :
                        Debug.Log("BLUETOOTH CSCD 2"+g.Call<AndroidJavaObject>("getDevice").Call<string>("getName")) ;
                        var isFind= g.Call<bool>("discoverServices") ;
                        break ;
                    case 3:
                        Debug.Log("BLUETOOTH CSCD 3") ;
                        break ;
                    default:
                        break ;
                }
            } ;
        
            bluetoothGattCallback.onServicesDiscoveredDelegate = (g, c) =>
            {
                var gattServices = g.Call<AndroidJavaObject>("getServices") ;
                Debug.Log("BLUETOOTH SDD..." + gattServices.Get<int>("size")) ;
                isReady = true ;
            } ;

            myBluetoothGattCallback.Call("AddUnityCallback", bluetoothGattCallback) ;
            BluetoothGatt = BluetoothDevice.Call<AndroidJavaObject>("connectGatt", activity, true, myBluetoothGattCallback) ;

            while (!isReady) ;
            Debug.Log("BLUETOOTH BEGIN SET UP") ;

            gattCharacteristic = GetCharacteristic(GetService(BluetoothGatt, serviceHRMID), characteristicHRM) ;
            NotificationData(BluetoothGatt, gattCharacteristic, descriptorID) ;

			GameObject.Find("SessionManager").GetComponent<ZenMasterSessionManager>().InitPrevisionStep();
        }
    }


    public AndroidJavaObject GetService(AndroidJavaObject gatt, string uuid)
    {
        return gatt.Call<AndroidJavaObject>("getService", ToAndroidUUID(uuid)) ;
    }

    public  AndroidJavaObject GetCharacteristic(AndroidJavaObject Service, string uuid)
    {
        return Service.Call<AndroidJavaObject>("getCharacteristic", ToAndroidUUID(uuid)) ;
    }

    public void NotificationData(AndroidJavaObject gatt, AndroidJavaObject characteristic, string uuid)
    {
        var b = gatt.Call<bool>("setCharacteristicNotification", characteristic, true) ;
        if (b)
        {
            var bluetoothGattDescriptor = new AndroidJavaClass("android.bluetooth.BluetoothGattDescriptor") ;
            var descriptor = characteristic.Call<AndroidJavaObject>("getDescriptor",ToAndroidUUID(uuid)) ;

            var  isSet = descriptor.Call<bool>("setValue", bluetoothGattDescriptor.GetStatic<byte[]>("ENABLE_NOTIFICATION_VALUE")) ;
            if (isSet)
            {
                var iSwrite = gatt.Call<bool>("writeDescriptor", descriptor) ;
            }
        }
    }

    public AndroidJavaObject ToAndroidUUID(string uuid)
    {
        var UUID = new AndroidJavaClass("java.util.UUID") ;
        return  UUID.CallStatic<AndroidJavaObject>("fromString", uuid) ;
    }
}