#import <AVFoundation/AVFoundation.h>
#import "ISN_Foundation.h"

extern "C" {
    
    int _ISN_AV_GetAuthorizationStatus(int mediaType) {
        
        AVMediaType type;
        
        switch (mediaType) {
            case 0:
                type = AVMediaTypeVideo;
                break;
            case 1:
                type = AVMediaTypeAudio;
                break;
            default:
                type = AVMediaTypeVideo;
                break;
        }
        
        AVAuthorizationStatus authStatus = [AVCaptureDevice authorizationStatusForMediaType:type];
        return authStatus;
    }
    
    
    void _ISN_AV_RequestAccessForMediaType(int mediaType) {
        
        AVMediaType type;
        
        switch (mediaType) {
            case 0:
                type = AVMediaTypeVideo;
                break;
            case 1:
                type = AVMediaTypeAudio;
                break;
            default:
                type = AVMediaTypeVideo;
                break;
        }
        
        [AVCaptureDevice requestAccessForMediaType:AVMediaTypeVideo completionHandler:^(BOOL granted) {
            int authStatus = [AVCaptureDevice authorizationStatusForMediaType:type];
            ISN_SendMessage(UNITY_AV_LISTENER, "OnRequestAccessCompleted", [NSString stringWithFormat:@"%d",authStatus]);
        }];
        
    }
    
   
    
    
}

