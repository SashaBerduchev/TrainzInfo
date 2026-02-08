using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class DocumentToIndex_WithoutICollections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentToIndex_StationImages_StationImagesid",
                table: "DocumentToIndex");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_DocumentToIndex_DocumentToIndexid",
                table: "NewsComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningUserRoutes_DocumentToIndex_DocumentToIndexid",
                table: "PlanningUserRoutes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningUserTrains_DocumentToIndex_DocumentToIndexid",
                table: "PlanningUserTrains");

            migrationBuilder.DropForeignKey(
                name: "FK_StationsShadules_DocumentToIndex_DocumentToIndexid",
                table: "StationsShadules");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainsShadule_DocumentToIndex_DocumentToIndexid",
                table: "TrainsShadule");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLocomotivePhotos_DocumentToIndex_DocumentToIndexid",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropIndex(
                name: "IX_UserLocomotivePhotos_DocumentToIndexid",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropIndex(
                name: "IX_TrainsShadule_DocumentToIndexid",
                table: "TrainsShadule");

            migrationBuilder.DropIndex(
                name: "IX_StationsShadules_DocumentToIndexid",
                table: "StationsShadules");

            migrationBuilder.DropIndex(
                name: "IX_PlanningUserTrains_DocumentToIndexid",
                table: "PlanningUserTrains");

            migrationBuilder.DropIndex(
                name: "IX_PlanningUserRoutes_DocumentToIndexid",
                table: "PlanningUserRoutes");

            migrationBuilder.DropIndex(
                name: "IX_NewsComments_DocumentToIndexid",
                table: "NewsComments");

            migrationBuilder.DropColumn(
                name: "DocumentToIndexid",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropColumn(
                name: "DocumentToIndexid",
                table: "TrainsShadule");

            migrationBuilder.DropColumn(
                name: "DocumentToIndexid",
                table: "StationsShadules");

            migrationBuilder.DropColumn(
                name: "DocumentToIndexid",
                table: "PlanningUserTrains");

            migrationBuilder.DropColumn(
                name: "DocumentToIndexid",
                table: "PlanningUserRoutes");

            migrationBuilder.DropColumn(
                name: "DocumentToIndexid",
                table: "NewsComments");

            migrationBuilder.RenameColumn(
                name: "StationImagesid",
                table: "DocumentToIndex",
                newName: "UserLocomotivePhotoId");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentToIndex_StationImagesid",
                table: "DocumentToIndex",
                newName: "IX_DocumentToIndex_UserLocomotivePhotoId");

            migrationBuilder.AddColumn<int>(
                name: "NewsCommentsId",
                table: "DocumentToIndex",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanningUserRouteID",
                table: "DocumentToIndex",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanningUserTrainID",
                table: "DocumentToIndex",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StationImageid",
                table: "DocumentToIndex",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StationsShaduleid",
                table: "DocumentToIndex",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainsShaduleid",
                table: "DocumentToIndex",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_NewsCommentsId",
                table: "DocumentToIndex",
                column: "NewsCommentsId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_PlanningUserRouteID",
                table: "DocumentToIndex",
                column: "PlanningUserRouteID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_PlanningUserTrainID",
                table: "DocumentToIndex",
                column: "PlanningUserTrainID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_StationImageid",
                table: "DocumentToIndex",
                column: "StationImageid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_StationsShaduleid",
                table: "DocumentToIndex",
                column: "StationsShaduleid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_TrainsShaduleid",
                table: "DocumentToIndex",
                column: "TrainsShaduleid");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentToIndex_NewsComments_NewsCommentsId",
                table: "DocumentToIndex",
                column: "NewsCommentsId",
                principalTable: "NewsComments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentToIndex_PlanningUserRoutes_PlanningUserRouteID",
                table: "DocumentToIndex",
                column: "PlanningUserRouteID",
                principalTable: "PlanningUserRoutes",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentToIndex_PlanningUserTrains_PlanningUserTrainID",
                table: "DocumentToIndex",
                column: "PlanningUserTrainID",
                principalTable: "PlanningUserTrains",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentToIndex_StationImages_StationImageid",
                table: "DocumentToIndex",
                column: "StationImageid",
                principalTable: "StationImages",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentToIndex_StationsShadules_StationsShaduleid",
                table: "DocumentToIndex",
                column: "StationsShaduleid",
                principalTable: "StationsShadules",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentToIndex_TrainsShadule_TrainsShaduleid",
                table: "DocumentToIndex",
                column: "TrainsShaduleid",
                principalTable: "TrainsShadule",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentToIndex_UserLocomotivePhotos_UserLocomotivePhotoId",
                table: "DocumentToIndex",
                column: "UserLocomotivePhotoId",
                principalTable: "UserLocomotivePhotos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentToIndex_NewsComments_NewsCommentsId",
                table: "DocumentToIndex");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentToIndex_PlanningUserRoutes_PlanningUserRouteID",
                table: "DocumentToIndex");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentToIndex_PlanningUserTrains_PlanningUserTrainID",
                table: "DocumentToIndex");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentToIndex_StationImages_StationImageid",
                table: "DocumentToIndex");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentToIndex_StationsShadules_StationsShaduleid",
                table: "DocumentToIndex");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentToIndex_TrainsShadule_TrainsShaduleid",
                table: "DocumentToIndex");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentToIndex_UserLocomotivePhotos_UserLocomotivePhotoId",
                table: "DocumentToIndex");

            migrationBuilder.DropIndex(
                name: "IX_DocumentToIndex_NewsCommentsId",
                table: "DocumentToIndex");

            migrationBuilder.DropIndex(
                name: "IX_DocumentToIndex_PlanningUserRouteID",
                table: "DocumentToIndex");

            migrationBuilder.DropIndex(
                name: "IX_DocumentToIndex_PlanningUserTrainID",
                table: "DocumentToIndex");

            migrationBuilder.DropIndex(
                name: "IX_DocumentToIndex_StationImageid",
                table: "DocumentToIndex");

            migrationBuilder.DropIndex(
                name: "IX_DocumentToIndex_StationsShaduleid",
                table: "DocumentToIndex");

            migrationBuilder.DropIndex(
                name: "IX_DocumentToIndex_TrainsShaduleid",
                table: "DocumentToIndex");

            migrationBuilder.DropColumn(
                name: "NewsCommentsId",
                table: "DocumentToIndex");

            migrationBuilder.DropColumn(
                name: "PlanningUserRouteID",
                table: "DocumentToIndex");

            migrationBuilder.DropColumn(
                name: "PlanningUserTrainID",
                table: "DocumentToIndex");

            migrationBuilder.DropColumn(
                name: "StationImageid",
                table: "DocumentToIndex");

            migrationBuilder.DropColumn(
                name: "StationsShaduleid",
                table: "DocumentToIndex");

            migrationBuilder.DropColumn(
                name: "TrainsShaduleid",
                table: "DocumentToIndex");

            migrationBuilder.RenameColumn(
                name: "UserLocomotivePhotoId",
                table: "DocumentToIndex",
                newName: "StationImagesid");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentToIndex_UserLocomotivePhotoId",
                table: "DocumentToIndex",
                newName: "IX_DocumentToIndex_StationImagesid");

            migrationBuilder.AddColumn<int>(
                name: "DocumentToIndexid",
                table: "UserLocomotivePhotos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentToIndexid",
                table: "TrainsShadule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentToIndexid",
                table: "StationsShadules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentToIndexid",
                table: "PlanningUserTrains",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentToIndexid",
                table: "PlanningUserRoutes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentToIndexid",
                table: "NewsComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLocomotivePhotos_DocumentToIndexid",
                table: "UserLocomotivePhotos",
                column: "DocumentToIndexid");

            migrationBuilder.CreateIndex(
                name: "IX_TrainsShadule_DocumentToIndexid",
                table: "TrainsShadule",
                column: "DocumentToIndexid");

            migrationBuilder.CreateIndex(
                name: "IX_StationsShadules_DocumentToIndexid",
                table: "StationsShadules",
                column: "DocumentToIndexid");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserTrains_DocumentToIndexid",
                table: "PlanningUserTrains",
                column: "DocumentToIndexid");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserRoutes_DocumentToIndexid",
                table: "PlanningUserRoutes",
                column: "DocumentToIndexid");

            migrationBuilder.CreateIndex(
                name: "IX_NewsComments_DocumentToIndexid",
                table: "NewsComments",
                column: "DocumentToIndexid");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentToIndex_StationImages_StationImagesid",
                table: "DocumentToIndex",
                column: "StationImagesid",
                principalTable: "StationImages",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_DocumentToIndex_DocumentToIndexid",
                table: "NewsComments",
                column: "DocumentToIndexid",
                principalTable: "DocumentToIndex",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningUserRoutes_DocumentToIndex_DocumentToIndexid",
                table: "PlanningUserRoutes",
                column: "DocumentToIndexid",
                principalTable: "DocumentToIndex",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningUserTrains_DocumentToIndex_DocumentToIndexid",
                table: "PlanningUserTrains",
                column: "DocumentToIndexid",
                principalTable: "DocumentToIndex",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_StationsShadules_DocumentToIndex_DocumentToIndexid",
                table: "StationsShadules",
                column: "DocumentToIndexid",
                principalTable: "DocumentToIndex",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainsShadule_DocumentToIndex_DocumentToIndexid",
                table: "TrainsShadule",
                column: "DocumentToIndexid",
                principalTable: "DocumentToIndex",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLocomotivePhotos_DocumentToIndex_DocumentToIndexid",
                table: "UserLocomotivePhotos",
                column: "DocumentToIndexid",
                principalTable: "DocumentToIndex",
                principalColumn: "id");
        }
    }
}
