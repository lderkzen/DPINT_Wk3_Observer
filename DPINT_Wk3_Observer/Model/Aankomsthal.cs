using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPINT_Wk3_Observer.Model
{
    public class Aankomsthal : IObserver<Baggageband>
    {
        public ObservableCollection<Vlucht> WachtendeVluchten { get; private set; }
        public List<Baggageband> Baggagebanden { get; private set; }

        public Aankomsthal()
        {
            WachtendeVluchten = new ObservableCollection<Vlucht>();
            Baggagebanden = new List<Baggageband>();

            Baggagebanden.Add(new Baggageband("Band 1", 30));
            Baggagebanden.Add(new Baggageband("Band 2", 60));
            Baggagebanden.Add(new Baggageband("Band 3", 90));

			Baggagebanden.ElementAt(0).Subscribe(this);
			Baggagebanden.ElementAt(1).Subscribe(this);
			Baggagebanden.ElementAt(2).Subscribe(this);
        }

        public void NieuweInkomendeVlucht(string vertrokkenVanuit, int aantalKoffers)
        {
			if (Baggagebanden.Any(bb => bb.AantalKoffers == 0) && !WachtendeVluchten.Any())
			{
				Baggageband legeBand = Baggagebanden.FirstOrDefault(bb => bb.AantalKoffers == 0);
				legeBand.HandelNieuweVluchtAf(new Vlucht(vertrokkenVanuit, aantalKoffers));
			}
			else
				WachtendeVluchten.Add(new Vlucht(vertrokkenVanuit, aantalKoffers));
		}

        public void WachtendeVluchtenNaarBand()
        {
            while(Baggagebanden.Any(bb => bb.AantalKoffers == 0) && WachtendeVluchten.Any())
            {                
                Baggageband legeBand = Baggagebanden.FirstOrDefault(bb => bb.AantalKoffers == 0);
                Vlucht volgendeVlucht = WachtendeVluchten.FirstOrDefault();
                WachtendeVluchten.RemoveAt(0);

                legeBand.HandelNieuweVluchtAf(volgendeVlucht);
            }
        }

		public void OnNext(Baggageband obj)
		{
			if (!(obj is null))
			{
				for (int i = 0; i < Baggagebanden.Count; i++)
				{
					if (Baggagebanden[i].Naam == obj.Naam)
					{
						if (Baggagebanden[i].AantalKoffers == 0 && WachtendeVluchten.Any())
						{
							Vlucht volgendeVlucht = WachtendeVluchten.FirstOrDefault();
							WachtendeVluchten.RemoveAt(0);
							Baggagebanden[i].HandelNieuweVluchtAf(volgendeVlucht);
						}
						return;
					}
				}
			}
		}

		public void OnError(Exception error)
		{
			throw new NotImplementedException();
		}

		public void OnCompleted() => throw new NotImplementedException();
	}
}
