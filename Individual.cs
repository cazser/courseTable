using System;
using Course;
namespace Algorithm
{
    public partial class Individual
    {

        private int[] chromosome;
        private double fitness = -1;

        //?原来java里的类是TimeTable我改成了课程表
        public Individual(CourseTable timetable)
        {
            int numClasses = timetable.GetNumClasses();

            // 1 gene for room, 1 for time, 1 for professor
            int chromosomeLength = numClasses * 3;
            // Create random individual
            int[] newChromosome = new int[chromosomeLength];
            int chromosomeIndex = 0;
            // Loop through groups
            foreach (Group group in timetable.GetGroupsAsArray())
            {
                // Loop through modules
                foreach (int moduleId in group.Modules)
                {
                    // Add random time
                    int timeslotId = timetable.GetRandomTimeslot().Id;
                    newChromosome[chromosomeIndex] = timeslotId;
                    chromosomeIndex++;

                    // Add random room
                    int roomId = timetable.GetRandomRoom().Id;
                    newChromosome[chromosomeIndex] = roomId;
                    chromosomeIndex++;

                    // Add random professor
                    Module module = timetable.GetModule(moduleId);
                    newChromosome[chromosomeIndex] = module.GetRandomTeacherId();
                    chromosomeIndex++;
                }
            }

            this.chromosome = newChromosome;
        }


        public Individual(int chromosomeLength)
        {
            // Create random individual
            int[] individual;
            individual = new int[chromosomeLength];

            /**
             * This comment and the for loop doesn't make sense for this chapter. But I'm
             * leaving it in here because you were instructed to copy this class from
             * Chapter 4 -- and NOT having this comment here might be more confusing than
             * keeping it in.
             * 
             * Comment from Chapter 4:
             * 
             * "In this case, we can no longer simply pick 0s and 1s -- we need to use every
             * city index available. We also don't need to randomize or shuffle this
             * chromosome, as crossover and mutation will ultimately take care of that for
             * us."
             */
            for (int gene = 0; gene < chromosomeLength; gene++)
            {
                individual[gene] = gene;
            }

            this.chromosome = individual;
        }

        /**
         * Initializes individual with specific chromosome
         * 
         * @param chromosome The chromosome to give individual
         */
        public Individual(int[] chromosome)
        {
            // Create individual chromosome
            this.chromosome = chromosome;
        }

        /**
         * Gets individual's chromosome
         * 
         * @return The individual's chromosome
         */
        public int[] Chromosome
        {
            get
            {
                return this.chromosome;
            }
        }

        /**
         * Gets individual's chromosome length
         * 
         * @return The individual's chromosome length
         */
        public int ChromosomeLength
        {
            get
            {
                return this.chromosome.Length;
            }
        }

        /**
         * Set gene at offset
         * 
         * @param gene
         * @param offset
         */
        public void SetGene(int offset, int gene)
        {
            this.chromosome[offset] = gene;
        }

        /**
         * Get gene at offset
         * 
         * @param offset
         * @return gene
         */
        public int GetGene(int offset)
        {
            return this.chromosome[offset];
        }


        public double Fitness
        {
            get
            {
                return this.fitness;
            }
            set { this.fitness = value; }
        }

        public override string ToString()
        {
            string output = "";
            for (int gene = 0; gene < this.chromosome.Length; gene++)
            {
                output += this.chromosome[gene] + ",";
            }
            return output;
        }

        /**
         * Search for a specific integer gene in this individual.
         * 
         * For instance, in a Traveling Salesman Problem where cities are encoded as
         * integers with the range, say, 0-99, this method will check to see if the city
         * "42" exists.
         * 
         * @param gene
         * @return
         */
        public bool ContainsGene(int gene)
        {
            for (int i = 0; i < this.chromosome.Length; i++)
            {
                if (this.chromosome[i] == gene)
                {
                    return true;
                }
            }
            return false;
        }

    }
}