using BusinessLogic.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic {
    public class BookingRules : IBookingRules{

        public (bool isValid, string errorMessage) ValidateAnimals(List<Animal> selectedAnimals, CustomerCard customerCard, DateTime bookingDate) {
            if(selectedAnimals.Count == 0) {
                return (false, "Je moet minimaal 1 beestje boeken.");
            }

            bool hasBoerderijdier = selectedAnimals.Any(a => a.AnimalType.TypeName == "Boerderij");
            if(hasBoerderijdier && (selectedAnimals.Any(a => a.Name == "Leeuw") || selectedAnimals.Any(a => a.Name == "IJsbeer"))) {
                return (false, "Je mag geen 'Leeuw' of 'IJsbeer' boeken als je ook een beestje boekt van het type 'Boerderijdier'.");
            }

            if(selectedAnimals.Any(a => a.Name == "Pinguïn" && (bookingDate.DayOfWeek == DayOfWeek.Saturday || bookingDate.DayOfWeek == DayOfWeek.Sunday))) {
                return (false, "Je mag geen beestje boeken met de naam 'Pinguïn' in het weekend.");
            }

            if(selectedAnimals.Any(a => a.AnimalType.TypeName == "Woestijn" && (bookingDate.Month >= 10 || bookingDate.Month <= 2))) {
                return (false, "Je mag geen beestje boeken van het type 'Woestijn' in de maanden oktober t/m februari.");
            }

            if(selectedAnimals.Any(a => a.AnimalType.TypeName == "Sneeuw" && (bookingDate.Month >= 6 && bookingDate.Month <= 8))) {
                return (false, "Je mag geen beestje boeken van het type 'Sneeuw' in de maanden juni t/m augustus.");
            }

            if(customerCard.CardType.Equals("None") && selectedAnimals.Count > 3) {
                return (false, "Klanten zonder klantenkaart mogen maximaal 3 dieren boeken.");
            }

            if(customerCard.CardType.Equals("Silver") && selectedAnimals.Count > 4) {
                return (false, "Klanten met een zilveren klantenkaart mogen maximaal 4 dieren boeken.");
            }

            if(!customerCard.CardType.Equals("Platinum")) {
                if(selectedAnimals.Any(a => a.AnimalType.TypeName == "VIP")) {
                    return (false, "Alleen klanten met een platina klantenkaart kunnen VIP dieren boeken.");
                }
            }

            if(customerCard.CardType.Equals("Gold")) {
                return (true, null);
            }

            return (true, null);
        }

        
    }
}
