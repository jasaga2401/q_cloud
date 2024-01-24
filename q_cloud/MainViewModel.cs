using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;
using q_cloud;

// this is the model-view-viewmodel class
namespace q_cloud
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _labelName;

        private string _labelDob;
        private DateTime _dateDob;

        private string _labelStart;
        private DateTime _dateStart;

        private string _labelQuit;
        private DateTime _dateQuit;

        private string _labelSmokeActiveDays;

        private string _labelSmokeFreeDays;

        private string _labelNoSmokes;

        private string _labelCostSaving;

        // handles the name of the user
        public string LabelName
        {
            get => _labelName;
            set
            {
                _labelName = value;
                OnPropertyChanged();
            }
        }

        // handles the date of birth of the user
        public string LabelDob
        {
            get => _labelDob;
            set
            {
                _labelDob = value;
                OnPropertyChanged();
            }
        }

        // handles the date started smoking
        public string LabelStart
        {
            get => _labelStart;
            set
            {
                _labelStart = value;
                OnPropertyChanged();
            }
        }

        // handles the quit date of the user
        public string LabelQuit
        {
            get => _labelQuit;
            set
            {
                _labelQuit = value;
                OnPropertyChanged();
            }
        }

        // handles the number of active days smoking
        public string LabelSmokingDays
        {
            get => _labelSmokeActiveDays;
            set
            {
                _labelSmokeActiveDays = value;
                OnPropertyChanged();
            }
        }

        // handles the number of smoke free days
        public string LabelSmokeFree
        {
            get => _labelSmokeFreeDays;
            set
            {
                _labelSmokeFreeDays = value;
                OnPropertyChanged();
            }
        }

        // handles the number of smokes not smoked
        public string LabelNoSmokes
        {
            get => _labelNoSmokes;
            set
            {
                _labelNoSmokes = value;
                OnPropertyChanged();
            }
        }

        // amount of money saved
        public string LabelCostSaving
        {
            get => _labelCostSaving;
            set
            {
                _labelCostSaving = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // 
        public MainViewModel()
        {
            // Retrieves the data from the database
            List<List<System.String>> entries = db_class.get_data();
            LabelName = entries[0][1] + " " + entries[0][2];

            // parsing the date of birth
            if (DateTime.TryParse(entries[0][3].ToString(), out DateTime parsedDate))
            {
                _dateDob = parsedDate;
            }
            else
            {
                // Handle parsing error or set a default value
                _dateDob = DateTime.MinValue; // or some other default
            }
            LabelDob = _dateDob.ToString("dd/MM/yyyy");


            // parsing the smoking start date
            if (DateTime.TryParse(entries[0][4].ToString(), out DateTime parsedDate1))
            {
                _dateStart = parsedDate1;
            }
            else
            {
                // Handle parsing error or set a default value
                _dateStart = DateTime.MinValue; // or some other default
            }
            LabelStart = _dateStart.ToString("dd/MM/yyyy");


            // parsing the quit date
            if (DateTime.TryParse(entries[0][5].ToString(), out DateTime parsedDateQuit))
            {
                _dateQuit = parsedDateQuit;
            }
            else
            {
                // Handle parsing error or set a default value
                _dateQuit = DateTime.MinValue; // or some other default
            }
            LabelQuit = _dateQuit.ToString("dd/MM/yyyy");

            // number of days smoking throughout your life

            // number of days alive 
            DateTime todayDate = DateTime.Now;
            TimeSpan daysAlive = todayDate - _dateDob;
            double daysAliveDec = daysAlive.TotalDays;

            // number of days smoking
            DateTime endSmokingDate = DateTime.Now;
            TimeSpan smokeActiveDays = endSmokingDate - _dateStart;
            double smokeActiveDaysInt = smokeActiveDays.TotalDays;
            string formattedSmokeActiveDays = smokeActiveDaysInt.ToString("N0");

            // proportion of days smoking
            double proportionSmoking = smokeActiveDaysInt / daysAliveDec;
            proportionSmoking *= 100;

            LabelSmokingDays = formattedSmokeActiveDays + "  (" + proportionSmoking.ToString("F1") + "%)";

            // smoke free days
            DateTime endDate = DateTime.Now;
            TimeSpan smokeFreeDays = endDate - _dateQuit;
            double smokeFreeDaysInt = smokeFreeDays.TotalDays;
            // string smokeFreeDaysString = smokeFreeDaysInt.ToString();
            LabelSmokeFree = smokeFreeDaysInt.ToString("F2");

            // number of smokes not smoked
            int averageSmokeDay = Int32.Parse(entries[0][6]);
            double noSmokes = smokeFreeDaysInt * averageSmokeDay;
            LabelNoSmokes = noSmokes.ToString("F2");

            // amount of money saved in terms of money / packets
            double costPacketCigarettes = double.Parse(entries[0][7]);
            double costSaving = (noSmokes / 20) * costPacketCigarettes;
            int packets = (int)(noSmokes / 20);
            if (packets == 1)
            {
                LabelCostSaving = costSaving.ToString("C2") + " (" + packets + " packet)";
            }
            else
            {
                LabelCostSaving = costSaving.ToString("C2") + " (" + packets + " packets)";
            }

        }
    }
}
