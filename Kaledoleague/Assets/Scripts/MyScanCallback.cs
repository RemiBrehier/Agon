using UnityEngine ;

public class MyScanCallback : AndroidJavaProxy
{
    public delegate void OnBatchScanResultsDelegate(AndroidJavaObject results) ;
    public OnBatchScanResultsDelegate onBatchScanResultsDelegate ;

    public delegate void OnScanFailedDelegate(int errorCode) ;
    public OnScanFailedDelegate onScanFailedDelegate ;

    public delegate void OnScanResultDelegate(int callbackType, AndroidJavaObject result) ;
    public OnScanResultDelegate onScanResultDelegate ;

    public MyScanCallback() : base("IScanCallback") { }

    public void onBatchScanResults(AndroidJavaObject results)
    {
        if (onBatchScanResultsDelegate != null)
        {
            onBatchScanResultsDelegate(results) ;
        }
    }

    public void onScanFailed(int errorCode)
    {
        if (onScanFailedDelegate != null)
        {
            onScanFailedDelegate(errorCode) ;
        }
    }
    
    public void onScanResult(int callbackType, AndroidJavaObject result)
    {
        if (onScanResultDelegate != null)
        {
            onScanResultDelegate(callbackType, result) ;
        }
    }
}

