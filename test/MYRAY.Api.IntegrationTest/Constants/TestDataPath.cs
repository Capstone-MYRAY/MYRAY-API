namespace MYRAY.Api.IntegrationTest.Constants;

public class TestDataPath
{
    private static string CurrentDirectory => Directory.GetCurrentDirectory();
    public static string AccountController => Path.Combine(CurrentDirectory, "TestData", "AccountController.json");
    public static string AreaController => Path.Combine(CurrentDirectory, "TestData", "AreaController.json");
    public static string AttendanceController => Path.Combine(CurrentDirectory, "TestData", "AttendanceController.json");
    public static string AuthenticationController => Path.Combine(CurrentDirectory, "TestData", "AuthenticationController.json");
    public static string BookmarkController => Path.Combine(CurrentDirectory, "TestData", "BookmarkController.json");
    public static string CommentController => Path.Combine(CurrentDirectory, "TestData", "CommentController.json");
    public static string ConfigController => Path.Combine(CurrentDirectory, "TestData", "ConfigController.json");
    public static string ExtendTaskJobController => Path.Combine(CurrentDirectory, "TestData", "ExtendTaskJobController.json");
    public static string FeedbackController => Path.Combine(CurrentDirectory, "TestData", "FeedbackController.json");
    public static string FileController => Path.Combine(CurrentDirectory, "TestData", "FileController.json");
    public static string GardenController => Path.Combine(CurrentDirectory, "TestData", "GardenController.json");
    public static string GuidepostController => Path.Combine(CurrentDirectory, "TestData", "GuidepostController.json");
    public static string JobPostController => Path.Combine(CurrentDirectory, "TestData", "JobPostController.json");
    public static string MessageController => Path.Combine(CurrentDirectory, "TestData", "MessageController.json");
    public static string PaymentHistoryController => Path.Combine(CurrentDirectory, "TestData", "PaymentHistoryController.json");
    public static string PostTypeController => Path.Combine(CurrentDirectory, "TestData", "PostTypeController.json");
    public static string ReportController => Path.Combine(CurrentDirectory, "TestData", "ReportController.json");
    public static string RoleController => Path.Combine(CurrentDirectory, "TestData", "RoleController.json");
    public static string TreeTypeController => Path.Combine(CurrentDirectory, "TestData", "TreeTypeController.json");
    public static string TriggerController => Path.Combine(CurrentDirectory, "TestData", "TriggerController.json");

}