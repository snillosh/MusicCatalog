import './App.css'
import {useState} from "react";
import {AlbumList} from "./components/AlbumList.tsx";
import {Group, Panel} from "react-resizable-panels";
import {AlbumDetails} from "./components/AlbumDetails.tsx";
import {ChildList} from "./components/ChildList.tsx";
import {useAlbums} from "./hooks/useAlbums.ts";
import {useAlbumTracks} from "./hooks/useAlbumTracks.ts";

function App() {

    const {data: apiAlbums = [], isLoading, error} = useAlbums();
    const [selectedAlbumId, setSelectedAlbumId] = useState<string | undefined>(undefined);
    const [selectedTrackId, setSelectedTrackId] = useState<string | undefined>(undefined);
    const {
        data: tracks = [],
        isLoading: tracksLoading,
    } = useAlbumTracks(selectedAlbumId);

    if (isLoading) {
        return <div>Loading albums...</div>;
    }

    if (error) {
        return <div>Failed to load albums.</div>;
    }

    console.log(apiAlbums);

    function onSelect(albumId: string) {
        setSelectedAlbumId(albumId);
        setSelectedTrackId(undefined);
    }

    function onSelectTrack(trackId: string) {
        setSelectedTrackId(trackId);
    }

    return (<Group>
        <Panel defaultSize={300} minSize={200}>
            <AlbumList albums={apiAlbums} selectedId={selectedAlbumId} onSelect={onSelect}/>
        </Panel>

        <Panel minSize={500}>
            <AlbumDetails albums={apiAlbums} selectedAlbumId={selectedAlbumId}/>
        </Panel>

        <Panel defaultSize={300} minSize={200}>
            {tracksLoading ? (
                <div>Loading tracks...</div>
            ) : (
                <ChildList
                    selectedTrackId={selectedTrackId}
                    tracks={tracks}
                    onSelectTrack={onSelectTrack}
                />
            )}
        </Panel>
    </Group>)
}

export default App
