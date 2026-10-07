// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using System.Linq;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using MergeIt.Core.FieldElements;

namespace MergeIt.Game.Helpers
{
    public static class FieldGeneratorElementExtensions
    {
        public static void UpdateGenerator(this IFieldElement generator)
        {
            if (generator.InfoParameters.Type != ElementType.Generator)
            {
                return;
            }

            ElementGeneratorSettings settings = generator.ConfigParameters.ElementConfig.GeneratorSettings;
            IGeneratorParameters parameters = generator.GeneratorParameters;
            IGeneratorOpenParameters openParameters = generator.GeneratorOpenParameters;
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (openParameters is {IsOpening: true})
            {
                long openTimePassed = currentTime - openParameters.StartOpeningTime;
                if (openTimePassed < settings.OpenTime)
                {
                    openParameters.RemainingTime.Value = settings.OpenTime - openTimePassed;

                    return;
                }

                generator.GeneratorOpenParameters = null;
            }

            if (parameters.StartChargingTime == 0)
            {
                return;
            }

            int availableToDrop = parameters.AvailableToDrop;
            int maxPossibleDrops = !settings.IsUnlimited ? settings.MaxDrop * settings.Stack : settings.MaxDrop;
            int maxAvailableToDrop = maxPossibleDrops - parameters.DroppedElements;
            maxAvailableToDrop = Math.Min(maxAvailableToDrop, settings.MaxDrop);
            int minDrop = Math.Min(settings.MinDrop, maxAvailableToDrop);

            long passedTime = currentTime - parameters.StartChargingTime;
            long restPassedTime = passedTime % settings.ChargeTime;
            int charged = (int)(passedTime / settings.ChargeTime) + parameters.ChargedCount;
            charged = Math.Min(charged, maxAvailableToDrop);

            parameters.MinDropFullChargeTime = minDrop * settings.ChargeTime;

            if (availableToDrop == 0)
            {
                if (charged >= minDrop)
                {
                    availableToDrop = charged;
                    parameters.ChargedCount = 0;
                    parameters.StartChargingTime = currentTime - restPassedTime;
                }
                else
                {
                    parameters.ChargedCount = charged;
                }
            }
            else
            {
                availableToDrop += charged;
                parameters.StartChargingTime = currentTime - restPassedTime;
                parameters.ChargedCount = 0;
            }

            availableToDrop = Math.Min(availableToDrop, maxAvailableToDrop);

            float restTime = settings.ChargeTime - restPassedTime;
            parameters.RemainChargeTime = restTime;

            if (availableToDrop == 0)
            {
                parameters.MinDropChargeTime.Value = (minDrop - 1 - charged) * settings.ChargeTime + restTime;
            }
            else if (availableToDrop < maxAvailableToDrop)
            {
                parameters.MinDropChargeTime.Value = 0;
            }
            else
            {
                parameters.MinDropChargeTime.Value = 0;
                parameters.RemainChargeTime = 0;
                parameters.StartChargingTime = 0;
            }
            
            parameters.AvailableToDrop = availableToDrop;
        }

        public static bool? TryGenerate(this IFieldElement generator)
        {
            if (generator.InfoParameters.Type != ElementType.Generator)
            {
                return null;
            }

            var settings = generator.ConfigParameters.ElementConfig.GeneratorSettings;
            var parameters = generator.GeneratorParameters;
            int maxPossibleDrops = !settings.IsUnlimited ? settings.MaxDrop * settings.Stack : settings.MaxDrop;

            parameters.AvailableToDrop--;

            if (!settings.IsUnlimited)
            {
                parameters.DroppedElements++;

                if (parameters.AvailableToDrop == 0 &&
                    parameters.DroppedElements >= maxPossibleDrops)
                {
                    return false;
                }
            }

            int availableToDrop = parameters.AvailableToDrop;
            int maxAvailableToDrop = maxPossibleDrops - parameters.DroppedElements;
            maxAvailableToDrop = Math.Min(maxAvailableToDrop, settings.MaxDrop);
            int minDrop = Math.Min(settings.MinDrop, maxAvailableToDrop);

            parameters.MinDropFullChargeTime = minDrop * settings.ChargeTime;

            if (!parameters.Charging)
            {
                parameters.StartChargingTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                parameters.RemainChargeTime = settings.ChargeTime;
            }

            if (availableToDrop == 0)
            {
                parameters.MinDropChargeTime.Value = (minDrop - 1 - parameters.ChargedCount) * settings.ChargeTime + parameters.RemainChargeTime;
            }
            
            return true;
        }

        public static void ChargeGenerator(this IFieldElement generator, int count = 1)
        {
            if (generator.InfoParameters.Type != ElementType.Generator)
            {
                return;
            }

            var settings = generator.ConfigParameters.ElementConfig.GeneratorSettings;
            var parameters = generator.GeneratorParameters;

            ChargeGenerator(parameters, settings, charged: count);
        }

        public static void SkipCharging(this IFieldElement generator)
        {
            if (generator.InfoParameters.Type != ElementType.Generator)
            {
                return;
            }

            var settings = generator.ConfigParameters.ElementConfig.GeneratorSettings;
            var parameters = generator.GeneratorParameters;

            int maxPossibleDrops = !settings.IsUnlimited ? settings.MaxDrop * settings.Stack : settings.MaxDrop;
            int maxAvailableToDrop = maxPossibleDrops - parameters.DroppedElements;
            int chargedCount = Math.Min(settings.MinDrop, maxAvailableToDrop);

            int timeDiff = settings.ChargeTime - (int)parameters.RemainChargeTime;
            
            parameters.MinDropChargeTime.Value = 0;
            parameters.ChargedCount = 0;
            parameters.StartChargingTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - timeDiff;
            parameters.AvailableToDrop = chargedCount;
        }

        public static void TrySkipOpeningTime(this IFieldElement generator, int seconds)
        {
            if (generator.InfoParameters.Type != ElementType.Generator)
            {
                return;
            }

            var parameters = generator.GeneratorOpenParameters;

            if (parameters is {IsOpening: true})
            {
                generator.GeneratorOpenParameters.RemainingTime.Value -= seconds;
            }
        }

        public static void TrySkipChargingTime(this IFieldElement generator, int seconds)
        {
            if (generator.InfoParameters.Type != ElementType.Generator)
            {
                return;
            }

            var settings = generator.ConfigParameters.ElementConfig.GeneratorSettings;
            var parameters = generator.GeneratorParameters;

            if (!parameters.Charging)
            {
                return;
            }

            int diff = (int)parameters.RemainChargeTime - seconds;
            parameters.MinDropChargeTime.Value -= seconds;

            if (diff < 0)
            {
                int chargedCount = 1;

                diff = Math.Abs(diff);
                int restCharged = diff / settings.ChargeTime;
                int restTime = diff % settings.ChargeTime;

                chargedCount += restCharged;

                ChargeGenerator(parameters, settings, chargedCount, restTime);
            }
            else
            {
                parameters.RemainChargeTime = diff;
                parameters.StartChargingTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - seconds;
            }
        }

        public static GeneratableElement GetCandidateToGenerate(this IFieldElement generator)
        {
            if (generator.InfoParameters.Type != ElementType.Generator)
            {
                return null;
            }

            ElementGeneratorSettings generatorSettings = generator.ConfigParameters.ElementConfig.GeneratorSettings;
            List<GeneratableElement> generatedItems = generatorSettings.GenerateItems;
            int sumPossibilities = generatedItems.Sum(x => x.Possibility);
            int random = UnityEngine.Random.Range(0, sumPossibilities);

            int min = 0;
            for (int i = 0; i < generatedItems.Count; i++)
            {
                var item = generatedItems[i];
                int max = min + item.Possibility;

                if (random >= min && random < max)
                {
                    return item;
                }

                min = max;
            }

            return null;
        }

        private static void ChargeGenerator(IGeneratorParameters parameters, ElementGeneratorSettings settings, int charged = 1, int skipTime = 0)
        {
            parameters.ChargedCount += charged;

            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            int availableToDrop = parameters.AvailableToDrop;
            int maxPossibleDrops = !settings.IsUnlimited ? settings.MaxDrop * settings.Stack : settings.MaxDrop;
            int maxAvailableToDrop = maxPossibleDrops - parameters.DroppedElements;
            maxAvailableToDrop = Math.Min(maxAvailableToDrop, settings.MaxDrop);
            int minDrop = Math.Min(settings.MinDrop, maxAvailableToDrop);

            parameters.RemainChargeTime = settings.ChargeTime - skipTime;
            parameters.MinDropFullChargeTime = minDrop * settings.ChargeTime;
            parameters.StartChargingTime = currentTime - skipTime;
            
            if (availableToDrop == 0 && parameters.ChargedCount >= minDrop ||
                availableToDrop > 0)
            {
                availableToDrop += parameters.ChargedCount;
               
                parameters.ChargedCount = 0;
            }

            availableToDrop = Math.Min(availableToDrop, maxAvailableToDrop);

            if (availableToDrop == maxAvailableToDrop)
            {
                parameters.StartChargingTime = 0;
                parameters.RemainChargeTime = 0;
                parameters.ChargedCount = 0;
            }

            parameters.AvailableToDrop = availableToDrop;
        }
    }
}