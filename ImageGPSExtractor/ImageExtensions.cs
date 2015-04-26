using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGPSExtractor
{
    /// <summary>
    /// Code taken from: 
    /// http://stackoverflow.com/questions/4983766/getting-gps-data-from-an-images-exif-in-c-sharp 
    /// http://denwilliams.net/2013/07/17/gps-metadata/
    /// </summary>
    public static class ImageExtensions
    {
        public static DateTime? GetDateTaken(this Image image)
        {
            try
            {
                //DateTimeOriginal
                PropertyItem propItem = image.GetPropertyItem(0x9003);
                // See also - DateTimeDigitized / CreateDate 0x9004
                // .. TimeZoneOffset 0x882a & ModifyDate (DateTime) 0x0132

                //Convert date taken metadata to a DateTime object
                string sdate = Encoding.UTF8.GetString(propItem.Value).Replace("\0", String.Empty).Trim();
                string secondhalf = sdate.Substring(sdate.IndexOf(" "), (sdate.Length - sdate.IndexOf(" ")));
                string firsthalf = sdate.Substring(0, 10);
                firsthalf = firsthalf.Replace(":", "-");
                sdate = firsthalf + secondhalf;
                return DateTime.Parse(sdate);
            }
            catch
            {
                return null;
            }
        }
        
        public static DateTime? GetGpsDateTimeStamp(this Image image)
        {
            try
            {
                //GPSTimeStamp
                PropertyItem propTime = image.GetPropertyItem(0x0007);
                double hours = GetExifSubValue(propTime, 0);
                double mins = GetExifSubValue(propTime, 1);
                double secs = GetExifSubValue(propTime, 2);
                string stime = string.Format("{0:00}:{1:00}:{2:00}", hours, mins, secs);

                //GPSDateStamp
                PropertyItem propDate = image.GetPropertyItem(0x001d);

                //Convert date taken metadata to a DateTime object
                string sdate = Encoding.UTF8.GetString(propDate.Value).Replace("\0", String.Empty).Trim();
                sdate = sdate.Replace(":", "-");
                return DateTime.Parse(sdate + " " + stime);
            }
            catch
            {
                return null;
            }
        }
        
        public static double? GetLatitude(this Image targetImg)
        {
            try
            {
                //Property Item 0x0001 - PropertyTagGpsLatitudeRef
                PropertyItem propItemRef = targetImg.GetPropertyItem(1);
                //Property Item 0x0002 - PropertyTagGpsLatitude
                PropertyItem propItemLat = targetImg.GetPropertyItem(2);
                return ExifGpsToFloat(propItemRef, propItemLat);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
        
        public static double? GetLongitude(this Image targetImg)
        {
            try
            {
                ///Property Item 0x0003 - PropertyTagGpsLongitudeRef
                PropertyItem propItemRef = targetImg.GetPropertyItem(3);
                //Property Item 0x0004 - PropertyTagGpsLongitude
                PropertyItem propItemLong = targetImg.GetPropertyItem(4);
                return ExifGpsToFloat(propItemRef, propItemLong);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
        
        public static double? GetAltitude(this Image targetImg)
        {
            try
            {
                //GPSAltitudeRef - 0 (above sea level) or 1 (below sea level)
                ///Property Item 0x0005 - PropertyTagGpsAltitudeRef
                PropertyItem propItemRef = targetImg.GetPropertyItem(5);
                //GPSAltitude
                //Property Item 0x0006 - PropertyTagGpsAltitude
                PropertyItem propItemLong = targetImg.GetPropertyItem(6);
                double value = GetExifSubValue(propItemLong, 0);
                if (propItemRef.Value[0] == 1)
                    value = 0 - value;
                return value;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
        
        public static GpsMetaData GetGpsInfo(this Image image)
        {
            double? lat = GetLatitude(image);
            double? lon = GetLongitude(image);
            var dTs = GetGpsDateTimeStamp(image);
            var alt = GetAltitude(image);
            var dTaken = GetDateTaken(image);
            var result = new GpsMetaData();
            if (lat.HasValue) result.Latitude = lat.Value;
            if (lon.HasValue) result.Longitude = lon.Value;
            if (alt.HasValue) result.Altitude = alt.Value;
            if (dTs.HasValue) result.Timestamp = dTs.Value;
            else if (dTaken.HasValue) result.Timestamp = dTaken.Value;

            return result;
        }
        
        private static double GetExifSubValue(PropertyItem property, int index)
        {
            int baseIndex = index * 8;
            uint numerator = BitConverter.ToUInt32(property.Value, baseIndex);
            uint denominator = BitConverter.ToUInt32(property.Value, baseIndex + 4);
            return numerator / denominator;
        }
        
        private static double ExifGpsToFloat(PropertyItem propItemRef, PropertyItem propItem)
        {
            uint degreesNumerator = BitConverter.ToUInt32(propItem.Value, 0);
            uint degreesDenominator = BitConverter.ToUInt32(propItem.Value, 4);
            double degrees = degreesNumerator / (double)degreesDenominator;

            uint minutesNumerator = BitConverter.ToUInt32(propItem.Value, 8);
            uint minutesDenominator = BitConverter.ToUInt32(propItem.Value, 12);
            double minutes = minutesNumerator / (double)minutesDenominator;

            uint secondsNumerator = BitConverter.ToUInt32(propItem.Value, 16);
            uint secondsDenominator = BitConverter.ToUInt32(propItem.Value, 20);
            double seconds = secondsNumerator / (double)secondsDenominator;

            double coorditate = degrees + (minutes / 60f) + (seconds / 3600f);
            string gpsRef = System.Text.Encoding.ASCII.GetString(new byte[1] { propItemRef.Value[0] }); //N, S, E, or W
            if (gpsRef == "S" || gpsRef == "W")
                coorditate = 0 - coorditate;
            return coorditate;
        }
    }

    /// <summary>
    /// GPS EXIF Metadata stored with geotagged images
    /// </summary>
    public class GpsMetaData
    {
        public GpsMetaData()
        {
            Latitude = Double.MinValue;
            Longitude = Double.MinValue;
            Altitude = Double.MinValue;
            Timestamp = DateTime.MinValue;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Altitude { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
