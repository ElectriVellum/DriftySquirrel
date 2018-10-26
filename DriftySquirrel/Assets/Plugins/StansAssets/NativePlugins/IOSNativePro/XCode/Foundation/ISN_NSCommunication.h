#import "JSONModel.h"
#import "ISN_Foundation.h"

#import <CoreLocation/CoreLocation.h>

@interface ISN_NSKeyValueObject : JSONModel
@property (nonatomic) NSString *m_key;
@property (nonatomic) NSString *m_value;

-(id) initWithData:(NSString *) key value:(NSString *) value;
@end

@interface  ISN_NSKeyValueResult : SA_Result
@property (nonatomic) ISN_NSKeyValueObject *m_keyValueObject;

-(id) initWithNSKeyValueObject:(ISN_NSKeyValueObject *) keyValueObject;
@end

@interface ISN_NSStoreDidChangeExternallyNotification : JSONModel
@property (nonatomic) int m_reason;
@property (nonatomic) NSArray<ISN_NSKeyValueObject *> *m_updatedData;
@end


//So far those models are only used for a user notifications API

@interface ISN_NSDateComponents : JSONModel
@property (nonatomic) long Hour;
@property (nonatomic) long Minute;
@property (nonatomic) long Second;
@property (nonatomic) long Nanosecond;

@property (nonatomic) long Year;
@property (nonatomic) long Month;
@property (nonatomic) long Day;

-(id) initWithNSDateComponents:(NSDateComponents *) date;
-(NSDateComponents *) getNSDateComponents;
@end


//Those should be moved to a core location as soon as we will have it

@interface ISN_CLLocationCoordinate2D : JSONModel
@property (nonatomic) float m_latitude;
@property (nonatomic) float m_longitude;

-(id) initWithCLLocationCoordinate2D:(CLLocationCoordinate2D ) location;
-(CLLocationCoordinate2D ) getCLLocationCoordinate2D;

@end

@interface ISN_CLCircularRegion : JSONModel
@property (nonatomic) NSString* m_identifier;
@property (nonatomic) bool m_notifyOnEntry;
@property (nonatomic) bool m_notifyOnExit;
@property (nonatomic) float m_radius;
@property (nonatomic) ISN_CLLocationCoordinate2D* m_center;

-(id) initWithCLLocationCoordinate2D:(CLCircularRegion *) region;
-(CLCircularRegion *) getCLCircularRegion;

@end


@protocol ISN_NSRange;
@interface ISN_NSRange : JSONModel

@property(nonatomic) long m_location;
@property(nonatomic) long m_length;

-(id) initWithNSRange:(NSRange ) range;
-(NSRange ) getNSRange;

@end

@protocol ISN_NSURL;
@interface ISN_NSURL : JSONModel
@property(nonatomic) NSString* m_url;
@property(nonatomic) int m_type;

-(NSURL* ) toNSURL;
@end

@protocol ISN_NSLocale;
@interface ISN_NSLocale : JSONModel
@property (nonatomic) NSString* m_identifier;
@property (nonatomic) NSString* m_countryCode;
@property (nonatomic) NSString* m_languageCode;
@property (nonatomic) NSString* m_currencySymbol;
@property (nonatomic) NSString* m_currencyCode;


-(id) initWithNSLocale:(NSLocale *) locale;
@end
