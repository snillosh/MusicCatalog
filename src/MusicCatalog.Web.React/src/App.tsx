import './App.css'
import {useState} from "react";
import type {Album} from "./models/album.ts";
import {AlbumList} from "./components/AlbumList.tsx";
import {Panel, Group} from "react-resizable-panels";
import {AlbumDetails} from "./components/AlbumDetails.tsx";
import {ChildList} from "./components/ChildList.tsx";
import type {Track} from "./models/track.ts";

function App() {

    const [albums] = useState<Album[]>([
        {
            id: 1,
            name: "Imaginal Disk",
            artist: "Magdalena Bay",
            releaseYear: 2024,
            rating: "⭐⭐⭐⭐⭐⭐",
            tracks: [
                { id: 1, position: 1, name: "She Looked Like Me!", duration: 324 },
                { id: 2, position: 2, name: "Killing Time", duration: 211 },
                { id: 3, position: 3, name: "Image", duration: 238 },
                { id: 4, position: 4, name: "Death & Romance", duration: 356 },
            ],
        },
        {
            id: 2,
            name: "Entertainment!",
            artist: "Gang of Four",
            releaseYear: 1979,
            rating: "⭐⭐⭐⭐",
            tracks: [
                { id: 5, position: 1, name: "Ether", duration: 231 },
                { id: 6, position: 2, name: "Natural's Not in It", duration: 186 },
                { id: 7, position: 3, name: "Not Great Men", duration: 187 },
                { id: 8, position: 4, name: "Damaged Goods", duration: 206 },
            ],
        },
        {
            id: 3,
            name: "In Rainbows",
            artist: "Radiohead",
            releaseYear: 2007,
            rating: "⭐⭐⭐⭐⭐",
            tracks: [
                { id: 9, position: 1, name: "15 Step", duration: 237 },
                { id: 10, position: 2, name: "Bodysnatchers", duration: 242 },
                { id: 11, position: 3, name: "Nude", duration: 255 },
                { id: 12, position: 4, name: "Weird Fishes/Arpeggi", duration: 318 },
            ],
        },
        {
            id: 4,
            name: "Discovery",
            artist: "Daft Punk",
            releaseYear: 2001,
            rating: "⭐⭐⭐⭐",
            tracks: [
                { id: 13, position: 1, name: "One More Time", duration: 320 },
                { id: 14, position: 2, name: "Aerodynamic", duration: 207 },
                { id: 15, position: 3, name: "Digital Love", duration: 301 },
                { id: 16, position: 4, name: "Harder, Better, Faster, Stronger", duration: 224 },
            ],
        },
        {
            id: 5,
            name: "The New Sound",
            artist: "Geordie Greep",
            releaseYear: 2024,
            rating: "⭐⭐⭐",
            tracks: [
                { id: 17, position: 1, name: "Blues", duration: 402 },
                { id: 18, position: 2, name: "Terra", duration: 387 },
                { id: 19, position: 3, name: "Holy, Holy", duration: 311 },
                { id: 20, position: 4, name: "The New Sound", duration: 428 },
            ],
        },
        {
            id: 6,
            name: "Remain in Light",
            artist: "Talking Heads",
            releaseYear: 1980,
            rating: "⭐⭐⭐⭐⭐",
            tracks: [
                { id: 21, position: 1, name: "Born Under Punches", duration: 349 },
                { id: 22, position: 2, name: "Crosseyed and Painless", duration: 285 },
                { id: 23, position: 3, name: "The Great Curve", duration: 406 },
                { id: 24, position: 4, name: "Once in a Lifetime", duration: 259 },
            ],
        },
        {
            id: 7,
            name: "To Pimp a Butterfly",
            artist: "Kendrick Lamar",
            releaseYear: 2015,
            rating: "⭐⭐⭐⭐⭐",
            tracks: [
                { id: 25, position: 1, name: "Wesley's Theory", duration: 287 },
                { id: 26, position: 2, name: "For Free?", duration: 130 },
                { id: 27, position: 3, name: "King Kunta", duration: 234 },
                { id: 28, position: 4, name: "These Walls", duration: 345 },
            ],
        },
        {
            id: 8,
            name: "Currents",
            artist: "Tame Impala",
            releaseYear: 2015,
            rating: "⭐⭐⭐",
            tracks: [
                { id: 29, position: 1, name: "Let It Happen", duration: 467 },
                { id: 30, position: 2, name: "Nangs", duration: 107 },
                { id: 31, position: 3, name: "The Moment", duration: 256 },
                { id: 32, position: 4, name: "Eventually", duration: 318 },
            ],
        },
    ]);
    const [selectedAlbumId, setSelectedAlbumId] = useState<number | undefined>(undefined);
    const [selectedTrackId, setSelectedTrackId] = useState<number | undefined>(undefined);
    const [tracks, setSelectedTracks] = useState<Track[] | undefined>(undefined);

    function onSelect(albumId : number)
    {
        setSelectedAlbumId(albumId);

        const tracks = albums.find(a => a.id === albumId)?.tracks;

        setSelectedTracks(tracks);
    }

    function onSelectTrack(trackId: number)
    {
        setSelectedTrackId(trackId);
    }



  return (<Group>
      <Panel defaultSize={300} minSize={200}>
              <AlbumList albums={albums} selectedId={selectedAlbumId} onSelect={onSelect}/>
      </Panel>

      <Panel minSize={500}>
          <AlbumDetails albums={albums} selectedAlbumId={selectedAlbumId}/>
      </Panel>

      <Panel defaultSize={300} minSize={200}>
              <ChildList selectedTrackId={selectedTrackId} tracks={tracks} onSelectTrack={onSelectTrack}/>
      </Panel>
  </Group>)
}

export default App
