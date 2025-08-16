using AIService.Domain;

namespace AIService.Infrastructure;

public class AIModelByName
{
    public static string GetModel(AIModel model)
    {
        switch (model)
        {
            case AIModel.Deepseek:
                return "deepseek/deepseek-chat-v3-0324";
        }

        return "none";
    }
}