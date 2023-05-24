# Datacap
### Install ###
1. Download the source code or use `git clone`.
2. If you have Visual Studio, open the .sln file, build the project, and run it.
3. Please contact me if you do not have Visual Studio and I can provide compiled binaries.

### Runtime instructions ###
1. The requested endpoints can be seen in the SwaggerUI.
2. Go to the Swagger UI 
3. The endpoint /api/Transaction will show the current leaderboard only. This ranks and displays the processors, their total fee, and their name.
4. The endpoint /api/Transaction/AllResults will show everything, including the leaderboard and all transactions used to calculate the leaderboard.
5. The endpoint /api/Transaction/void will use the void file to recalculate the leaderboard and will only show the leaderboard.
