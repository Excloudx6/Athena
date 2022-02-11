from mythic_payloadtype_container.MythicCommandBase import *
import json
from mythic_payloadtype_container.MythicRPC import *


class RmArguments(TaskArguments):
    def __init__(self, command_line, **kwargs):
        super().__init__(command_line, **kwargs)
        self.args = [
            CommandParameter(
                name="path",
                type=ParameterType.String,
                description="Path to file to remove",
                parameter_group_info=[ParameterGroupInfo()]
            ),
            CommandParameter(
                name="recurse",
                type=ParameterType.Boolean,
                description="If in a directory, force remove all files within the directory",
                default_value = False,
                parameter_group_info=[ParameterGroupInfo(
                    required=False
                )]
            ),
        ]

    async def parse_arguments(self):
        if len(self.command_line) > 0:
            if self.command_line[0] == "{":
                temp_json = json.loads(self.command_line)
                if "host" in temp_json:
                    self.add_arg("path", temp_json["path"] + "/" + temp_json["file"])
                else:
                    self.add_arg("path", temp_json["path"])
            else:
                self.add_arg("path", self.command_line)
        else:
            raise ValueError("Missing arguments")

class RmCommand(CommandBase):
    cmd = "rm"
    needs_admin = False
    help_cmd = "rm [path]"
    description = "Remove a file"
    version = 1
    supported_ui_features = ["file_browser:remove"]
    author = "@checkymander"
    attackmapping = ["T1106", "T1070.004"]
    argument_class = RmArguments

    async def create_tasking(self, task: MythicTask) -> MythicTask:
        resp = MythicRPC().execute("create_artifact", task_id=task.id,
            artifact="fileManager.removeItemAtPathError",
            artifact_type="API Called",
        )
        return task

    async def process_response(self, response: AgentResponse):
        pass