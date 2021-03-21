using Fishing.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fishing.ViewModels
{
    public class OnboardingViewModel : BaseViewModel
    {
        private ObservableRangeCollection<OnboardingModel> items;

        public ObservableRangeCollection<OnboardingModel> Items { get => items;
            set => SetProperty(ref items, value);
        }


        public OnboardingViewModel()
        {
            // create our dummy onboarding items
            Items = new ObservableRangeCollection<OnboardingModel>
            {
                new OnboardingModel
                {
                    Title = "Discover nature and catch the fish",
                    Content = "Crazy houndshark with pipefish snake eel. The crackin, heart pumping at Snaggletooth shark asian clam, banded eel algae applesnail lobster, lionfish tilefish banded sole an spot hogfish. Parrotfish at crazy houndshark with pipefish snake eel.",
                    Image  = "OnboardImage1"
                },
                new OnboardingModel
                {
                    Title = "Find new fishing spots",
                    Content = "Bonnethead at puffer fish pipefish octopus threadfin. Floating ear snail fishy grow, amazing cold blooded seabass goatfish lionfish painted comber. Coral hogfish at Bursa trigger spot hogfish bite yellow pseudochromis weasel shark seabass i.",
                    Image  = "OnboardImage2"
                },
                new OnboardingModel
                {
                    Title = "Who is fishing near me",
                    Content = "Gold damsel faucet snail, in snake eel sea coral grouper. Mystery snail lionfish papershell houndshark. Puffer fish stingray, bicolor blenny and quickly moving, stingray menacing crab flounder, black clown goby hammerhead with lobster crawling butterflyfish. In the coral threadfin hawkfish. Milk shark swim Asian clam soldierfish. Parrotfish a.",
                    Image  = "OnboardImage3"
                },
                new OnboardingModel
                {
                    Title = "Find new fishing spots",
                    Content = "Bonnethead at puffer fish pipefish octopus threadfin. Floating ear snail fishy grow, amazing cold blooded seabass goatfish lionfish painted comber. Coral hogfish at Bursa trigger spot hogfish bite yellow pseudochromis weasel shark seabass i.",
                    Image  = "OnboardImage2"
                },
            };


        }

    }
}
