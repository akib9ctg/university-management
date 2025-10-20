using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseAndClassNameConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                WITH sanitized AS (
                    SELECT "Id",
                           LEFT(
                               CASE
                                   WHEN regexp_replace("Name", '[^A-Za-z0-9]', '', 'g') = ''
                                       THEN substr(translate("Id"::text, '-', ''), 1, 12)
                                   ELSE regexp_replace("Name", '[^A-Za-z0-9]', '', 'g')
                               END,
                               200
                           ) AS base_name,
                           row_number() OVER (
                               PARTITION BY
                                   LEFT(
                                       CASE
                                           WHEN regexp_replace("Name", '[^A-Za-z0-9]', '', 'g') = ''
                                               THEN substr(translate("Id"::text, '-', ''), 1, 12)
                                           ELSE regexp_replace("Name", '[^A-Za-z0-9]', '', 'g')
                                       END,
                                       200
                                   )
                               ORDER BY "Id"
                           ) AS rn
                    FROM "Courses"
                )
                UPDATE "Courses" c
                SET "Name" = LEFT(
                        sanitized.base_name ||
                        CASE
                            WHEN sanitized.rn > 1
                                THEN substr(translate(c."Id"::text, '-', ''), 1, 6)
                            ELSE ''
                        END,
                        200
                    )
                FROM sanitized
                WHERE c."Id" = sanitized."Id";
                """);

            migrationBuilder.Sql(
                """
                WITH sanitized AS (
                    SELECT "Id",
                           LEFT(
                               CASE
                                   WHEN regexp_replace("Name", '[^A-Za-z0-9]', '', 'g') = ''
                                       THEN substr(translate("Id"::text, '-', ''), 1, 12)
                                   ELSE regexp_replace("Name", '[^A-Za-z0-9]', '', 'g')
                               END,
                               100
                           ) AS base_name,
                           row_number() OVER (
                               PARTITION BY
                                   LEFT(
                                       CASE
                                           WHEN regexp_replace("Name", '[^A-Za-z0-9]', '', 'g') = ''
                                               THEN substr(translate("Id"::text, '-', ''), 1, 12)
                                           ELSE regexp_replace("Name", '[^A-Za-z0-9]', '', 'g')
                                       END,
                                       100
                                   )
                               ORDER BY "Id"
                           ) AS rn
                    FROM "Classes"
                )
                UPDATE "Classes" c
                SET "Name" = LEFT(
                        sanitized.base_name ||
                        CASE
                            WHEN sanitized.rn > 1
                                THEN substr(translate(c."Id"::text, '-', ''), 1, 6)
                            ELSE ''
                        END,
                        100
                    )
                FROM sanitized
                WHERE c."Id" = sanitized."Id";
                """);

                migrationBuilder.CreateIndex(
                    name: "IX_Courses_Name",
                    table: "Courses",
                    column: "Name",
                    unique: true);

                migrationBuilder.CreateIndex(
                    name: "IX_Classes_Name",
                    table: "Classes",
                    column: "Name",
                    unique: true);

                migrationBuilder.AddCheckConstraint(
                    name: "CK_Courses_Name_Alphanumeric",
                    table: "Courses",
                    sql: "\"Name\" ~ '^[A-Za-z0-9]+$'");

                migrationBuilder.AddCheckConstraint(
                    name: "CK_Classes_Name_Alphanumeric",
                    table: "Classes",
                    sql: "\"Name\" ~ '^[A-Za-z0-9]+$'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Courses_Name_Alphanumeric",
                table: "Courses");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Classes_Name_Alphanumeric",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Courses_Name",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Classes_Name",
                table: "Classes");
        }
    }
}
