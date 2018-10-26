#import "ISN_NSCommunication.h"

@implementation ISN_NSKeyValueResult
    -(id) init { return self = [super init]; }
    -(id) initWithNSKeyValueObject:(ISN_NSKeyValueObject *) keyValueObject {
        self = [super init];
        if(self) {
            self.m_keyValueObject = keyValueObject;
        }
        return self;
    }
@end

@implementation ISN_NSKeyValueObject
    -(id) init { return self = [super init]; }
    -(id) initWithData:(NSString *) key value:(NSString *) value {
        self = [super init];
        if(self) {
            self.m_key = key;
            self.m_value = value;
        }
        return self;
    }
@end

@implementation ISN_NSStoreDidChangeExternallyNotification
-(id) init {
    self = [super init];
    if(self) {
        self.m_reason = -1;
    }
    
    return self;
}
@end

//So far those models are only used for a user notifications API


@implementation ISN_NSDateComponents

-(id) initWithNSDateComponents:(NSDateComponents *) date {
    self = [super init];
    if(self) {
        
        self.Hour = date.hour;
        self.Minute = date.minute;
        self.Second = date.second;
        self.Nanosecond = date.nanosecond;
       
        self.Year = date.year;
        self.Month = date.month;
        self.Day = date.day;

    }
    return self;
}
-(NSDateComponents *) getNSDateComponents {
    NSDateComponents* date = [[NSDateComponents alloc] init];
    if(self.Hour != 0) {date.hour = self.Hour; }
    if(self.Minute != 0) {date.minute = self.Minute; }
    if(self.Second != 0) {date.second = self.Second; }
    if(self.Nanosecond != 0) {date.nanosecond = self.Nanosecond; }
    
    if(self.Year != 0) {date.year = self.Year; }
    if(self.Month != 0) {date.month = self.Month; }
    if(self.Day != 0) {date.day = self.Day; }
    
    return  date;
}
@end

//Those should be moved to a core location as soon as we will have it


@implementation ISN_CLLocationCoordinate2D

-(id) initWithCLLocationCoordinate2D:(CLLocationCoordinate2D ) location {
    self = [super init];
    if(self) {
        self.m_latitude = location.latitude;
        self.m_longitude = location.longitude;
    }
    return self;
}
-(CLLocationCoordinate2D) getCLLocationCoordinate2D {
   return  CLLocationCoordinate2DMake(self.m_latitude, self.m_longitude);
}

@end

@implementation ISN_CLCircularRegion

-(id) initWithCLLocationCoordinate2D:(CLCircularRegion *) region {
    
    self = [super init];
    if(self) {
        self.m_identifier = region.identifier;
        self.m_notifyOnEntry = region.notifyOnEntry;
        self.m_notifyOnExit = region.notifyOnExit;
        self.m_radius = region.radius;
        self.m_center = [[ISN_CLLocationCoordinate2D alloc] initWithCLLocationCoordinate2D:region.center];
    }
    return self;
}
-(CLCircularRegion *) getCLCircularRegion {
    
    CLCircularRegion* region = [[CLCircularRegion alloc]
                                initWithCenter:[self.m_center getCLLocationCoordinate2D]
                                radius:self.m_radius identifier:
                                self.m_identifier];
   
    region.notifyOnExit = self.m_notifyOnExit;
    region.notifyOnEntry = self.m_notifyOnEntry;
    
    return  region;
}
@end



@implementation ISN_NSRange : JSONModel


-(id) initWithNSRange:(NSRange) range {
    self = [super init];
    if(self) {
        self.m_length = range.length;
        self.m_location = range.location;
    }
    return self;
}


-(NSRange) getNSRange {
    return NSMakeRange(self.m_location, self.m_length);
}

@end

@implementation ISN_NSURL : JSONModel
-(NSURL* ) toNSURL {
    switch (self.m_type) {
        case 1: //File
            return [NSURL fileURLWithPath:self.m_url];
            break;
        default:
            return [NSURL URLWithString:self.m_url];
            break;
    }
}

@end



@implementation ISN_NSLocale : JSONModel

-(id) initWithNSLocale:(NSLocale *) locale {
    self = [super init];
    if(self) {
        if(locale != NULL) {
            self.m_identifier       = [locale objectForKey:NSLocaleIdentifier];
            self.m_countryCode      = [locale objectForKey:NSLocaleCountryCode];
            self.m_languageCode     = [locale objectForKey:NSLocaleLanguageCode];
            self.m_currencySymbol   = [locale objectForKey:NSLocaleCurrencySymbol];
            self.m_currencyCode     = [locale objectForKey:NSLocaleCurrencyCode];
            
        }
    }
    return self;
}

@end


