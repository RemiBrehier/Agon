import android.bluetooth.le.ScanResult ;
import java.util.List ;

public interface IScanCallback
{
    public abstract void AddUnityCallback (IScanCallback isc) ;
    public abstract void onBatchScanResults(List<ScanResult> results) ;
    public abstract void onScanFailed(int errorCode) ;
    public abstract void onScanResult(int callbackType, ScanResult result) ;
}
