using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Configuration.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangedStoredData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_roles_name",
                schema: "identity",
                table: "roles",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_roles_name",
                schema: "identity",
                table: "roles");
        }
    }
}
