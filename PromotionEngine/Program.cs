using System;
using System.Collections.Generic;
using PromotionEngine.Models;
using PromotionEngine.Repositories;

namespace PromotionEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var controller = new Controller(new PromotionRepository(), new PriceRepository());

            var scenario1Items = new List<Item>
            {
                new("A", 1),
                new("B", 1),
                new("C", 1)
            };

            Console.WriteLine($"Scenario A: \n 1 * A \n 1 * B \n 1 * C \nTotal: {controller.CalculateTotal(scenario1Items)}\n");

            var scenario2Items = new List<Item>
            {
                new("A", 5),
                new("B", 5),
                new("C", 1)
            };

            Console.WriteLine($"Scenario B: \n 5 * A \n 5 * B \n 1 * C \nTotal: {controller.CalculateTotal(scenario2Items)}\n");

            var scenario3Items = new List<Item>
            {
                new("A", 3),
                new("B", 5),
                new("C", 1),
                new("D", 1)
            };

            Console.WriteLine($"Scenario C: \n 3 * A \n 5 * B \n 1 * C \n1 * D \nTotal: {controller.CalculateTotal(scenario3Items)}\n");

        }


    }
}
