using CakeFactory.Service.Settings;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Timers;

namespace CakeFactory.Service
{
    public class CakeService : ICakeService
    {
        private readonly ConcurrentBag<Cake> _cakes;
        private readonly ReportingTimer _reportingTimer;
        private readonly ExecutionDataflowBlockOptions _prepareOptions;
        private readonly ExecutionDataflowBlockOptions _cookOptions;
        private readonly ExecutionDataflowBlockOptions _packageOptions;
        private readonly DurationSettings _durationSettingsModel;
        public CakeService(DurationSettings settings)
        {
            _cakes = new ConcurrentBag<Cake>();
            _durationSettingsModel = settings;
            _prepareOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = settings.PrepareMaxDegree };
            _cookOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = settings.CookMaxDegree };
            _packageOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = settings.PackageMaxDegree };
            _reportingTimer = new ReportingTimer(settings.ReportingDuration, OnReportingTimerEvent);
        }
        public async Task RunAsync(Stock stock)
        {
            _reportingTimer?.Start();

            //Step 1: Get the list of Ingredients to prepare the cake 
            var prepareStep = new TransformBlock<Recipe, Cake>(async recipe =>
            {
                var creationDate = DateTime.Now;
                await Task.Delay(_durationSettingsModel.PrepareDuration);
                var cake = new Cake(recipe)
                {
                    CreationDate = creationDate
                };
                _cakes.Add(cake);
                return cake;
            }, _prepareOptions);

            //Step 2: Cook the prepared Cake
            var cookStep = new TransformBlock<Cake, Cake>(async cake =>
            {
                await Task.Delay(_durationSettingsModel.CookDuration);
                cake.Status = CakeStatus.Cooked;
                return cake;
            }, _cookOptions);

            //Step 3: package the cooked Cake
            var packageStep = new TransformBlock<Cake, Cake>(async cake =>
            {
                await Task.Delay(_durationSettingsModel.PackageDuration);
                cake.Status = CakeStatus.Packaged;
                return cake;
            }, _packageOptions);

            //Step 4: End of pipeline
            var deliveryStep = new ActionBlock<Cake>(async cake =>
            {
                await Task.Delay(_durationSettingsModel.DeliveryDuration);
                cake.Status = CakeStatus.Delivered;
                cake.DeliveryDate = DateTime.Now;
            });

            var _linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
            prepareStep.LinkTo(cookStep, _linkOptions);
            cookStep.LinkTo(packageStep, _linkOptions);
            packageStep.LinkTo(deliveryStep, _linkOptions);

            while (true)
            {
                var recipe = await stock.GetNextRecipeAsync();
                if (recipe == null)
                {
                    break;
                }

                await prepareStep.SendAsync(recipe);
            }

            // Mark the head of the pipeline as complete.
            prepareStep.Complete();

            // Wait for the last block in the pipeline to process all messages.
            deliveryStep.Completion.Wait();
            _reportingTimer?.Stop();
        }
        private void OnReportingTimerEvent(object sender, ElapsedEventArgs e)
        {
            var prepared = _cakes.Count(x => x.Status == CakeStatus.Prepared);
            var cooked = _cakes.Count(x => x.Status == CakeStatus.Cooked);
            var packaged = _cakes.Count(x => x.Status == CakeStatus.Packaged);
            var delivered = _cakes.Count(x => x.Status == CakeStatus.Delivered);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"État de traitement au: {DateTime.Now}");
            Console.WriteLine($"Total des gâteaux terminés: {delivered}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"- Gâteaux préparés: {prepared}");
            Console.WriteLine($"- Gâteaux cuits: {cooked}");
            Console.WriteLine($"- Gâteaux emballés: {packaged}\n");
            Console.ResetColor();
        }
    }
}
