using System;
namespace Algorithm
{
    public partial class Population
    {
        private Individual[] population;
        private double populationFitness = -1;

        public Population(int populationSize)
        {
            // Initial population
            this.population = new Individual[populationSize];
        }
        public Population(int populationSize, CourseTable timetable)
        {
            // Initial population
            this.population = new Individual[populationSize];

            // Loop over population size
            for (int individualCount = 0; individualCount < populationSize; individualCount++)
            {
                // Create individual
                Individual individual = new Individual(timetable);
                // Add individual to population
                this.population[individualCount] = individual;
            }
        }

        public Population(int populationSize, int chromosomeLength)
        {
            // Initial population
            this.population = new Individual[populationSize];

            // Loop over population size
            for (int individualCount = 0; individualCount < populationSize; individualCount++)
            {
                // Create individual
                Individual individual = new Individual(chromosomeLength);
                // Add individual to population
                this.population[individualCount] = individual;
            }
        }
        public Individual[] GetIndividuals()
        {
            return this.population;
        }
        public double PopulationFitness
        {
            get { return this.populationFitness; }
            set
            {
                this.populationFitness = value;
            }
        }


        public Individual GetFittest(int offset)
        {
            Array.Sort(this.population);
            // Return the fittest individual
            return this.population[offset];
        }

        public int Size()
        {
            return this.population.Length;
        }

        public Individual SetIndividual(int offset, Individual individual)
        {
            return population[offset] = individual;
        }

        public Individual GetIndividual(int offset)
        {
            return population[offset];
        }

        public void shuffle()
        {
            Random rnd = new Random();
            for (int i = population.Length - 1; i > 0; i--)
            {
                int index = rnd.Next(i + 1);
                Individual a = population[index];
                population[index] = population[i];
                population[i] = a;
            }
        }


    }//&end of class Population
}
//?end of Algorithm.population



