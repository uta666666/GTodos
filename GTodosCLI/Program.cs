using GTodosLib;
using System;
using System.IO;

namespace GTodosCLI
{
    class Program
    {
        static string ApplicationName = "GTodos Application";

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Google Tasks APIを使用してTODOリストを管理します...\n");

            try
            {
                var googleTasksService = new GoogleTasksService(ApplicationName);
                
                // 認証
                await googleTasksService.AuthenticateAsync();
                Console.WriteLine("認証情報が token.json に保存されました\n");

                while (true)
                {
                    Console.WriteLine("=== メニュー ===");
                    Console.WriteLine("1. タスクリストを表示");
                    Console.WriteLine("2. タスクを登録");
                    Console.WriteLine("3. 終了");
                    Console.Write("\n選択してください (1-3): ");
                    
                    string? choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            await DisplayTaskLists(googleTasksService);
                            break;
                        case "2":
                            await CreateTask(googleTasksService);
                            break;
                        case "3":
                            Console.WriteLine("\n終了します...");
                            return;
                        default:
                            Console.WriteLine("\n無効な選択です。もう一度選択してください。\n");
                            break;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("エラー: credentials.json ファイルが見つかりません");
                Console.WriteLine("\n以下の手順でGoogle Cloud Consoleから認証情報を取得してください:");
                Console.WriteLine("1. https://console.cloud.google.com/ にアクセス");
                Console.WriteLine("2. プロジェクトを作成または選択");
                Console.WriteLine("3. 「APIとサービス」→「認証情報」に移動");
                Console.WriteLine("4. 「認証情報を作成」→「OAuthクライアントID」を選択");
                Console.WriteLine("5. アプリケーションの種類として「デスクトップアプリ」を選択");
                Console.WriteLine("6. ダウンロードしたJSONファイルを 'credentials.json' として保存");
                Console.WriteLine("7. Google Tasks APIを有効化してください");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラーが発生しました: {ex.Message}");
            }

            Console.WriteLine("\nEnterキーを押して終了...");
            Console.ReadLine();
        }



        /// <summary>
        /// Displays the task lists and their associated tasks retrieved from the specified Google Tasks service.
        /// </summary>
        /// <remarks>This method retrieves up to 10 task lists and, for each task list, retrieves up to 20
        /// tasks.  It displays the title, ID, and tasks for each task list. For each task, the status, title, notes, 
        /// and due date (if available) are displayed. If no task lists or tasks are found, appropriate messages  are
        /// displayed.</remarks>
        /// <param name="googleTasksService">An instance of <see cref="GoogleTasksService"/> used to retrieve task lists and tasks.</param>
        /// <returns></returns>
        static async System.Threading.Tasks.Task DisplayTaskLists(GoogleTasksService googleTasksService)
        {
            Console.WriteLine("\n=== TODOリスト ===\n");
            
            // タスクリストの取得
            var taskLists = await googleTasksService.GetTaskListsAsync(maxResults: 10);

            if (taskLists?.Items != null && taskLists.Items.Count > 0)
            {
                foreach (var taskList in taskLists.Items)
                {
                    Console.WriteLine($"📋 リスト: {taskList.Title}");
                    Console.WriteLine($"   ID: {taskList.Id}");
                    Console.WriteLine();

                    // 各リストのタスクを取得
                    var tasks = await googleTasksService.GetTasksAsync(taskList.Id, maxResults: 20);

                    if (tasks?.Items != null && tasks.Items.Count > 0)
                    {
                        foreach (var task in tasks.Items)
                        {
                            string status = task.Status.ToLower() == "completed" ? "✅" : "⬜";
                            Console.WriteLine($"   {status} {task.Title}");
                            
                            if (!string.IsNullOrEmpty(task.Notes))
                            {
                                Console.WriteLine($"      メモ: {task.Notes}");
                            }
                            
                            if (task.Due != null)
                            {
                                Console.WriteLine($"      期限: {task.Due}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("   タスクはありません");
                    }
                    
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("TODOリストが見つかりませんでした\n");
            }
        }

        /// <summary>
        /// Creates a new task in a selected task list using the provided Google Tasks service.
        /// </summary>
        /// <remarks>This method allows the user to select a task list from the available task lists,
        /// input task details  such as title, notes, and due date, and then create the task in the selected list. The
        /// user is prompted  for input via the console. If no task lists are available, or if the input is invalid, the
        /// method  terminates without creating a task.  The due date, if provided, must be in the format "yyyy-MM-dd".
        /// If the format is invalid, the task will  be created without a due date. Any exceptions encountered during
        /// task creation are caught and logged  to the console.</remarks>
        /// <param name="googleTasksService">An instance of <see cref="GoogleTasksService"/> used to interact with the Google Tasks API.</param>
        /// <returns></returns>
        static async System.Threading.Tasks.Task CreateTask(GoogleTasksService googleTasksService)
        {
            Console.WriteLine("\n=== タスクを登録 ===\n");

            // タスクリストの取得と選択
            var taskLists = await googleTasksService.GetTaskListsAsync(maxResults: 10);

            if (taskLists?.Items == null || taskLists.Items.Count == 0)
            {
                Console.WriteLine("タスクリストが見つかりませんでした\n");
                return;
            }

            Console.WriteLine("登録先のタスクリストを選択してください:");
            for (int i = 0; i < taskLists.Items.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {taskLists.Items[i].Title}");
            }
            Console.Write("\n番号を入力: ");
            
            if (!int.TryParse(Console.ReadLine(), out int listChoice) || 
                listChoice < 1 || listChoice > taskLists.Items.Count)
            {
                Console.WriteLine("無効な選択です\n");
                return;
            }

            var selectedTaskList = taskLists.Items[listChoice - 1];

            // タスク情報の入力
            Console.Write("\nタスクのタイトル: ");
            string? title = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("タイトルは必須です\n");
                return;
            }

            Console.Write("メモ (省略可): ");
            string? notes = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(notes))
            {
                notes = null;
            }

            Console.Write("期限 (yyyy-MM-dd形式、省略可): ");
            string? dueInput = Console.ReadLine();
            string? due = null;
            
            if (!string.IsNullOrWhiteSpace(dueInput))
            {
                if (DateTime.TryParse(dueInput, out DateTime dueDate))
                {
                    due = dueDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                }
                else
                {
                    Console.WriteLine("日付の形式が正しくありません。期限なしで登録します。");
                }
            }

            // タスクを作成
            try
            {
                var createdTask = await googleTasksService.CreateTaskAsync(
                    selectedTaskList.Id,
                    title,
                    notes,
                    due
                );

                Console.WriteLine("\n✅ タスクを登録しました!");
                Console.WriteLine($"タスク名: {createdTask.Title}");
                if (!string.IsNullOrEmpty(createdTask.Notes))
                {
                    Console.WriteLine($"メモ: {createdTask.Notes}");
                }
                if (createdTask.Due != null)
                {
                    Console.WriteLine($"期限: {createdTask.Due}");
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nタスクの登録に失敗しました: {ex.Message}\n");
            }
        }
    }
}
