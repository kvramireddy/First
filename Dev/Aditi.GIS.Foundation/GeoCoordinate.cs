using System;
namespace Aditi.GIS.Foundation
{
    public struct GeoCoordinate : IEquatable<GeoCoordinate>
    {
        private float _lon;
        private float _lat;

        public GeoCoordinate(float lat, float lon)
            : this()
        {
            this._lat = lat;
            this._lon = lon;
        }

        public float Lat
        {
            get
            {
                return this._lat;
            }

            set
            {
                this._lat = value;
            }
        }

        public float Lon
        {
            get
            {
                return this._lon;
            }

            set
            {
                this._lon = value;
            }
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", this.Lat, this.Lon);
        }

        public override int GetHashCode()
        {
            return this.Lat.GetHashCode() ^ this.Lon.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is GeoCoordinate)
            {
                GeoCoordinate other = (GeoCoordinate)obj;
                return this == other;
            }

            return false;
        }

        public bool Equals(GeoCoordinate other)
        {
            return this == other;
        }

        public static bool operator ==(GeoCoordinate left, GeoCoordinate right)
        {
            return left.Lat.Equals(right.Lat) && left.Lon.Equals(right.Lon);
        }

        public static bool operator !=(GeoCoordinate left, GeoCoordinate right)
        {
            return !left.Lat.Equals(right.Lat) || !left.Lon.Equals(right.Lon);
        }
    }
}
