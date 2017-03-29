# TorrentSwitch: Tie multiple torrent managers into one simple tool
A program that links multiple torrent clients into a simple manager. This allows users to add multiple torrent managers which are running on different systems/devices.
Adding multiple instances of the same torrent manager are possible, allowing the user to set multiple download folders, something many torrent managers do not support.

<p align="center">
<img src ="http://i.imgur.com/eq1qINE.png">
</p>

## Usage
TorrentSwitch can be associated with torrent files and magnet links.
On first start a client or multiple clients need to be added so the program have something to work with. 
Each new client creates a new column in the main dataGrid. 
When openning a torrent or magnet link the program recognizes it and loads the file/link. From there the user can choose where to forward the torrent from the avalaible clients.

This project is based on the [BencodeNet library](https://github.com/Krusen/BencodeNET).

##What this program does not do 
Download files/torrents

## Installation

The release comes with a installer. 

## Features
Torrent files supported

Magnet links supported

Drag-N-Drop supported

Loading torrents as arguments



## Supported Torrent Clients
| Clients        | Send torrent | Set label  | Set custom path  |
| ------------- |:-------------:| -----:|-----:|
| uTorrent      | <img height="20" width="20" src ="TorrentSwitch/Image/online.png"> | <img height="20" width="20" src ="TorrentSwitch/Image/offline.png"> | <img height="20" width="20" src ="TorrentSwitch/Image/offline.png"> |
| Deluge      | <img height="20" width="20" src ="TorrentSwitch/Image/online.png"> |   <img height="20" width="20" src ="TorrentSwitch/Image/online.png">|  <img height="20" width="20" src ="TorrentSwitch/Image/offline.png">  |
| Transmission | <img height="20" width="20" src ="TorrentSwitch/Image/online.png"> |     <img height="20" width="20" src ="TorrentSwitch/Image/offline.png"> |   <img height="20" width="20" src ="TorrentSwitch/Image/offline.png"> |
| qBittorrent | <img height="20" width="20" src ="TorrentSwitch/Image/offline.png"> |   <img height="20" width="20" src ="TorrentSwitch/Image/offline.png"> |  <img height="20" width="20" src ="TorrentSwitch/Image/offline.png"> |

## Future planned torrent clients
qBittorrent

Contact me if you have a client you would like added.

## Issues
if you find any problem contact me or try to fix it and I will merge your pull request




