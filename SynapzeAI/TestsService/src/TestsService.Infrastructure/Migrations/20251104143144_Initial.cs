using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestsService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    unique_user_name = table.Column<string>(type: "text", nullable: false),
                    test_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    theme = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    seconds = table.Column<int>(type: "integer", nullable: true),
                    minutes = table.Column<int>(type: "integer", nullable: true),
                    is_published = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "saved_tests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    test_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_saved_tests", x => x.id);
                    table.ForeignKey(
                        name: "fk_saved_tests_tests_test_id",
                        column: x => x.test_id,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "solving_histories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    unique_user_name = table.Column<string>(type: "text", nullable: false),
                    user_telegram = table.Column<string>(type: "text", nullable: false),
                    solving_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    solving_time_seconds = table.Column<int>(type: "integer", nullable: false),
                    test_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_solving_histories", x => x.id);
                    table.ForeignKey(
                        name: "fk_solving_histories_tests_test_id",
                        column: x => x.test_id,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    serial_number = table.Column<int>(type: "integer", nullable: false),
                    task_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    task_message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    right_answer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    answers = table.Column<string>(type: "jsonb", nullable: true),
                    test_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tasks", x => x.id);
                    table.ForeignKey(
                        name: "fk_tasks_tests_test_id",
                        column: x => x.test_id,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_histories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    serial_number = table.Column<int>(type: "integer", nullable: false),
                    task_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    task_message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    right_answer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    answers = table.Column<string>(type: "jsonb", nullable: true),
                    user_answer = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    points = table.Column<int>(type: "integer", nullable: true),
                    solving_history_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_histories", x => x.id);
                    table.ForeignKey(
                        name: "fk_task_histories_solving_histories_solving_history_id",
                        column: x => x.solving_history_id,
                        principalTable: "solving_histories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_statistics",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    errors_count = table.Column<int>(type: "integer", nullable: false),
                    right_answers_count = table.Column<int>(type: "integer", nullable: false),
                    last_review_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    avg_time_solving_sec = table.Column<float>(type: "real", nullable: false),
                    task_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_statistics", x => x.id);
                    table.ForeignKey(
                        name: "fk_task_statistics_tasks_task_id",
                        column: x => x.task_id,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_saved_tests_test_id",
                table: "saved_tests",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "ix_solving_histories_test_id",
                table: "solving_histories",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_histories_solving_history_id",
                table: "task_histories",
                column: "solving_history_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_statistics_task_id",
                table: "task_statistics",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasks_test_id",
                table: "tasks",
                column: "test_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "saved_tests");

            migrationBuilder.DropTable(
                name: "task_histories");

            migrationBuilder.DropTable(
                name: "task_statistics");

            migrationBuilder.DropTable(
                name: "solving_histories");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "tests");
        }
    }
}
