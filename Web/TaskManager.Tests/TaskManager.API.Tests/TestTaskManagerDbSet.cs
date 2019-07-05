using System.Linq;

namespace TaskManager.API.Tests
{
    class TestTaskManagerDbSet : TestDbSet<Models.Task>
    {
        public override Models.Task Find(params object[] keyValues)
        {
            return this.SingleOrDefault(
                task => task.Id == (int)keyValues.Single());
        }
    }
}
