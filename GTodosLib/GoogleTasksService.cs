using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using System.IO;
using System.Threading;

namespace GTodosLib
{
    public class GoogleTasksService
    {
        private static string[] Scopes = { TasksService.Scope.Tasks };
        private readonly string _applicationName;
        private TasksService? _service;

        public GoogleTasksService(string applicationName)
        {
            _applicationName = applicationName;
        }

        public async System.Threading.Tasks.Task AuthenticateAsync(string credentialsPath = "credentials.json", string tokenPath = "token.json")
        {
            UserCredential credential;

            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new Google.Apis.Util.Store.FileDataStore(tokenPath, true));
            }

            _service = new TasksService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });
        }

        public async System.Threading.Tasks.Task<TaskLists?> GetTaskListsAsync(int maxResults = 10)
        {
            if (_service == null)
                throw new InvalidOperationException("サービスが初期化されていません。AuthenticateAsync()を先に呼び出してください。");

            TasklistsResource.ListRequest listRequest = _service.Tasklists.List();
            listRequest.MaxResults = maxResults;

            return await listRequest.ExecuteAsync();
        }

        public async System.Threading.Tasks.Task<Tasks?> GetTasksAsync(string taskListId, int maxResults = 20)
        {
            if (_service == null)
                throw new InvalidOperationException("サービスが初期化されていません。AuthenticateAsync()を先に呼び出してください。");

            TasksResource.ListRequest taskRequest = _service.Tasks.List(taskListId);
            taskRequest.MaxResults = maxResults;

            return await taskRequest.ExecuteAsync();
        }

        public async System.Threading.Tasks.Task<Google.Apis.Tasks.v1.Data.Task?> CreateTaskAsync(
            string taskListId, 
            string title, 
            string? notes = null, 
            string? due = null)
        {
            if (_service == null)
                throw new InvalidOperationException("サービスが初期化されていません。AuthenticateAsync()を先に呼び出してください。");

            var newTask = new Google.Apis.Tasks.v1.Data.Task
            {
                Title = title,
                Notes = notes,
                Due = due
            };

            TasksResource.InsertRequest insertRequest = _service.Tasks.Insert(newTask, taskListId);
            return await insertRequest.ExecuteAsync();
        }
    }
}
