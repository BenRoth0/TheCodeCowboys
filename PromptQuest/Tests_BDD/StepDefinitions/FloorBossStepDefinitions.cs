using System;
using Reqnroll;

namespace Tests_BDD.StepDefinitions
{
    [Binding]
    public class FloorBossStepDefinitions
    {
        [Given("I beat the {int}th room")]
        public void GivenIBeatTheThRoom(int p0)
        {
            throw new PendingStepException();
        }

        [When("I move to the {int}th room")]
        public void WhenIMoveToTheThRoom(int p0)
        {
            throw new PendingStepException();
        }

        [Then("A boss should be spawned")]
        public void ThenABossShouldBeSpawned()
        {
            throw new PendingStepException();
        }
    }
}
