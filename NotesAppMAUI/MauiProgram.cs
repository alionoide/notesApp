using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using NotesAppAPI;
using NotesAppMAUI.View;
using NotesAppMAUI.ViewModel;

namespace NotesAppMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons-Regular");
                });

#if DEBUG
		    builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton(typeof(INotesAPI), typeof(NotesAPI));
            builder.Services.AddTransient(typeof(SubjectList));
            builder.Services.AddTransient(typeof(SubjectListVM));

            builder.Services.AddTransient(typeof(Subject));
            builder.Services.AddTransient(typeof(SubjectVM));
            builder.Services.AddTransient(typeof(Goal));
            builder.Services.AddTransient(typeof(GoalVM));
            builder.Services.AddTransient(typeof(TaskItem));
            builder.Services.AddTransient(typeof(TaskItemVM));

            builder.Services.AddTransient(typeof(Profile));
            builder.Services.AddTransient(typeof(ProfileVM));


            return builder.Build();
        }
    }
}