using System;

namespace Console_Application_C_ {
    public class Entity {
        public string fName {
            set;
            get;
        }

        public string lName {
            set;
            get;
        }

        public string country {
            set;
            get;
        }

        public string city {
            set;
            get;
        }

        public int score {
            set;
            get;
        }

        public Entity(string _fname, string _lname, string _country, string _city, int _score) {
            fName = _fname;
            lName = _lname;
            country = _country;
            city = _city;
            score = _score;
        }

        public string toString() {
            return String.Concat(
                this.fName + ";", 
                this.lName + ";",
                this.country + ";",
                this.city + ";",
                this.score.ToString()
            );
        }
    }

    public class CountryEntity {
        public string country;
        public float avg;
        public float median;
        public int max;
        public string maxPerson;
        public int min;
        public string minPerson;
        public int records;

        public CountryEntity(
            string _country,
            float _avg, 
            float _med, 
            int _max, 
            string _maxP, 
            int _min, 
            string _minP, 
            int _rec
        ) {
            country = _country;
            avg = _avg;
            median = _med;
            max = _max;
            maxPerson = _maxP;
            min = _min;
            minPerson = _minP;
            records = _rec;
        }

        public string toString() {
            return String.Concat(
                this.country + ";",
                this.avg.ToString() + ";", 
                this.median.ToString() + ";",
                this.max.ToString() + ";",
                this.maxPerson + ";",
                this.min.ToString() + ";",
                this.minPerson + ";",
                this.records.ToString()
            );
        }
    }
}