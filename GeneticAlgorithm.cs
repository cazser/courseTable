using System;
namespace Algorithm
{
    public partial class GeneticAlgorithm
    {
        private int populationSize;
        private double mutationRate;
        private double crossoverRate;
        private int elitismCount;
        protected int tournamentSize;

        public GeneticAlgorithm(int populationSize, double mutationRate, double crossoverRate, int elitismCount,
                int tournamentSize)
        {

            this.populationSize = populationSize;
            this.mutationRate = mutationRate;
            this.crossoverRate = crossoverRate;
            this.elitismCount = elitismCount;
            this.tournamentSize = tournamentSize;
        }

        public Population InitPopulation(CourseTable timetable)
        {
            // Initialize population
            Population population = new Population(this.populationSize, timetable);
            return population;
        }

        public bool IsTerminationConditionMet(int generationsCount, int maxGenerations)
        {
            return (generationsCount > maxGenerations);
        }


        public bool IsTerminationConditionMet(Population population)
        {
            return population.GetFittest(0).Fitness == 1.0;
        }

        public double CalcFitness(Individual individual, CourseTable timetable)
        {

            // Create new timetable object to use -- cloned from an existing timetable
            CourseTable threadTimetable = new CourseTable(timetable);
            threadTimetable.CreateClasses(individual);

            // Calculate fitness
            int clashes = threadTimetable.CalcClashes();
            double fitness = 1 / (double)(clashes + 1);

            individual.Fitness = fitness;

            return fitness;
        }

        public void EvalPopulation(Population population, CourseTable timetable)
        {
            double populationFitness = 0;

            // Loop over population evaluating individuals and summing population
            // fitness
            foreach (Individual individual in population.GetIndividuals())
            {
                populationFitness += this.CalcFitness(individual, timetable);
            }

            population.PopulationFitness = populationFitness;
        }
        public Individual SelectParent(Population population)
        {
            // Create tournament
            Population tournament = new Population(this.tournamentSize);

            // Add random individuals to the tournament
            population.Shuffle();
            for (int i = 0; i < this.tournamentSize; i++)
            {
                Individual tournamentIndividual = population.GetIndividual(i);
                tournament.SetIndividual(i, tournamentIndividual);
            }

            // Return the best
            return tournament.GetFittest(0);
        }

        public Population MutatePopulation(Population population, CourseTable timetable)
        {
            // Initialize new population
            Population newPopulation = new Population(this.populationSize);
            var rand = new Random();
            // Loop over current population by fitness
            for (int populationIndex = 0; populationIndex < population.Size(); populationIndex++)
            {
                Individual individual = population.GetFittest(populationIndex);

                // Create random individual to swap genes with
                Individual randomIndividual = new Individual(timetable);

                // Loop over individual's genes
                for (int geneIndex = 0; geneIndex < individual.ChromosomeLength; geneIndex++)
                {
                    // Skip mutation if this is an elite individual
                    if (populationIndex > this.elitismCount)
                    {
                        // Does this gene need mutation?
                        if (this.mutationRate > rand.NextDouble())
                        {
                            // Swap for new gene
                            individual.SetGene(geneIndex, randomIndividual.GetGene(geneIndex));
                        }
                    }
                }

                // Add individual to population
                newPopulation.SetIndividual(populationIndex, individual);
            }

            // Return mutated population
            return newPopulation;
        }
        public Population CrossoverPopulation(Population population)
        {
            // Create new population
            Population newPopulation = new Population(population.Size());
            var rand = new Random();
            // Loop over current population by fitness
            for (int populationIndex = 0; populationIndex < population.Size(); populationIndex++)
            {
                Individual parent1 = population.GetFittest(populationIndex);

                // Apply crossover to this individual?
                if (this.crossoverRate > rand.NextDouble() && populationIndex >= this.elitismCount)
                {
                    // Initialize offspring
                    Individual offspring = new Individual(parent1.ChromosomeLength);

                    // Find second parent
                    Individual parent2 = SelectParent(population);

                    // Loop over genome
                    for (int geneIndex = 0; geneIndex < parent1.ChromosomeLength; geneIndex++)
                    {
                        // Use half of parent1's genes and half of parent2's genes
                        if (0.5 > rand.NextDouble())
                        {
                            offspring.SetGene(geneIndex, parent1.GetGene(geneIndex));
                        }
                        else
                        {
                            offspring.SetGene(geneIndex, parent2.GetGene(geneIndex));
                        }
                    }

                    // Add offspring to new population
                    newPopulation.SetIndividual(populationIndex, offspring);
                }
                else
                {
                    // Add individual to new population without applying crossover
                    newPopulation.SetIndividual(populationIndex, parent1);
                }
            }

            return newPopulation;
        }





    }
}//&end of Algorithm.GeneticAlgorithm
