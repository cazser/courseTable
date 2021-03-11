using System;
//!课程模块的抽象,绑定老师（Teacher)与课程(Module)

namespace Course
{
    public partial class Module
    {
        private int moduleId;
        private string moduleCode;
        private string module;
        private int[] teacherIds;

        public Module(int moduleId, string moduleCode, string module, int[] teacherIds)
        {
            this.moduleId = moduleId;
            this.moduleCode = moduleCode;
            this.module = module;
            this.teacherIds = teacherIds;
        }

        public int ModuleId
        {
            get
            {
                return this.moduleId;
            }
        }


        public string ModuleCode
        {
            get { return this.moduleCode; }
        }

        public string ModuleName
        {
            get { return this.module; }
        }


        public int GetRandomTeacherId()
        {
            var rand = new Random();
            int randIndex = rand.Next(teacherIds.Length);
            int teacherId = this.teacherIds[randIndex];
            return teacherId;
        }
    }
}