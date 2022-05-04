using System.Collections;
using System.Collections.Generic;
using UnityEngine ;

using UnityEngine.Android ;

public class BLEDevice
{
    public string name = "" ;
    public string address = "" ;

    public BLEDevice (string _name, string _address)
    {
        name = _name ;
        address = _address ;
    }
}

public class BLEScan  : MonoBehaviour
{
    AndroidJavaObject bluetoothAdapter ;
    AndroidJavaObject myScanCallback ;
    private List<BLEDevice> bleDevices ;

    public string filterName = "" ;
    public string filterMac = "" ;

	bool isInitialized = false ;

    void Start()
    {
        bleDevices = new List <BLEDevice>() ;
        
        var bluetooth = new AndroidJavaClass("android.bluetooth.BluetoothAdapter") ;
        bluetoothAdapter = bluetooth.CallStatic<AndroidJavaObject>("getDefaultAdapter") ;

        if (bluetoothAdapter == null)
        {
            Debug.Log("ScanCallBack Error >> " + "Bluetooth Adapter seems unreachable") ;
            return ;
        }
        else
        {
            InitScannerCallback() ;
            var isOpen = OpenDevice() ;            
        }
            
    }

    bool OpenDevice()
    {
        if (!bluetoothAdapter.Call<bool>("isEnabled"))
        {
            var isOpen = bluetoothAdapter.Call<bool>("enable") ;
            if (!isOpen)
            {
                Debug.Log("ScanCallBack Error >> " + "Bluetooth Adapter seems disabled") ;
                OpenDevice() ;
            }
        }
        return bluetoothAdapter.Call<bool>("isEnabled") ;
    }

    void InitScannerCallback()
    {
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation) ;
        }
        #endif

        var bleScanner = bluetoothAdapter.Call<AndroidJavaObject>("getBluetoothLeScanner") ;
        
		myScanCallback = new AndroidJavaObject("MyScanCallback") ;

        MyScanCallback scanCallback = new MyScanCallback() ;
        
        scanCallback.onScanResultDelegate = (t, r) =>
        {
            if (r == null)
            {
                return ;
            }

            var bluetoothDevice = r.Call<AndroidJavaObject>("getDevice") ;
            var deviceName = bluetoothDevice.Call<string>("getName") ;
            var deviceAddress = bluetoothDevice.Call<string>("getAddress") ;

			if ( string.IsNullOrEmpty(deviceName) || string.IsNullOrEmpty(deviceAddress))
                return ;
			if (deviceName.Contains("hBAND") || deviceName.Contains("Polar"))
	            addDevice(deviceName, deviceAddress) ;
        } ;

        myScanCallback.Call("AddUnityCallback", scanCallback) ;
		isInitialized = true ;   
    }

    public void StartScan()
    {
		if (isInitialized)
		{
			var bleScanner = bluetoothAdapter.Call<AndroidJavaObject>("getBluetoothLeScanner") ;
			bleScanner.Call("startScan", myScanCallback) ;
		}
    }

    public void StopScan()
    {
        var bleScanner = bluetoothAdapter.Call<AndroidJavaObject>("getBluetoothLeScanner") ;
        bleScanner.Call("stopScan", myScanCallback) ;
    }

    void addDevice(string name, string address)
    {
        bool isPresent = false ;
        foreach (BLEDevice device in bleDevices)
        {
            if (device.name == name && device.address == address)
                isPresent = true ;
        }

        if (!isPresent)
        {
            bleDevices.Add(new BLEDevice(name, address)) ;
            Debug.Log("ScanCallBack Result >> " + "device " + name + " with address " + address + " was added." ) ;
            Debug.Log("ScanCallBack Result >> " + "that's " + bleDevices.Count + " devices on our list." ) ;
        }
    }

    public List<BLEDevice> GetScannedDevices()
    {
        return bleDevices ;
    }

}
