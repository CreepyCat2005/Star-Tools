# Information

This Project is currently under development in a very early stage. It is intended as universal Star-Citizen platform for every information out there about the game. It uses my own API to get the lastest information about the game. You may ask now why i am making my own API even if there are so many outside i could use? Well, here are a few reasons why i use my own API:
* Better Perfomance
* Way more accurate data (in most cases)
* Full control about security and serverload
* Quicker bugfixes at the API side
* And more...

# Why is it Public?

I am developing everything by myself including **Frontend, Backend and the Databasemanager**. The big problem about the whole thing is CIG and their update policy. Since nobody knows when the next update will be released i have to code everything in advance and maybe guess the next big content updates. If i wouldn't do that, it may take weeks or months to bring the newest informations and features into the Software. 
But you may think now, "haven't you mentioned a database updater?". Yes, i have a databse tool that updates the database every 24h to the newest available informations BUT this only affects the pure data. If CIG is going to bring Salvaging into the game with update 3.18 the Frontend would not be able to process / display the data.
