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
                    test_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    theme = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    with_ai = table.Column<bool>(type: "boolean", nullable: false),
                    limit_time_seconds = table.Column<int>(type: "integer", nullable: true),
                    limit_time_minutes = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletion_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "solving_histories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_histories = table.Column<string>(type: "jsonb", nullable: false),
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
                    task_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    task_message = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    right_answer = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    image_path = table.Column<string>(type: "text", nullable: true),
                    audio_path = table.Column<string>(type: "text", nullable: true),
                    task_statistic_errors_count = table.Column<int>(type: "integer", nullable: true),
                    task_statistic_right_answers_count = table.Column<int>(type: "integer", nullable: true),
                    task_statistic_last_review_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    task_statistic_avg_time_solving_sec = table.Column<float>(type: "real", nullable: true),
                    answers = table.Column<string>(type: "jsonb", nullable: true),
                    test_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletion_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "ix_solving_histories_test_id",
                table: "solving_histories",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasks_test_id",
                table: "tasks",
                column: "test_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "solving_histories");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "tests");
        }
    }
}
