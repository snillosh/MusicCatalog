import './App.css'
import {useState} from "react";
import {Group as PanelGroup, Panel} from "react-resizable-panels";
import {useAlbums} from "./hooks/useAlbums.ts";
import {useAlbumTracks} from "./hooks/useAlbumTracks.ts";
import {AppShell, Group, Space, Title} from "@mantine/core";
import {AlbumList} from "./components/AlbumList.tsx";
import {AlbumDetails} from "./components/AlbumDetails.tsx";
import {ChildList} from "./components/ChildList.tsx";
import {MetadataSearchBox} from "./components/MetadataSearchBox.tsx";
import {AddAlbumModel} from "./components/AddAlbumModel.tsx";

function App() {
    const {data: apiAlbums = [], isLoading, error} = useAlbums();
    const [selectedAlbumId, setSelectedAlbumId] = useState<string | undefined>(undefined);
    const [selectedTrackId, setSelectedTrackId] = useState<string | undefined>(undefined);
    const {
        data: tracks = [],
        isLoading: tracksLoading,
    } = useAlbumTracks(selectedAlbumId);

    const [selectedReleaseGroupId, setSelectedReleaseGroupId] =
        useState<string>();

    const [importModalOpen, setImportModalOpen] =
        useState(false);

    if (error) {
        return <div>Failed to load albums.</div>;
    }

    if (isLoading) {
        return <div>Loading albums...</div>;
    }

    if (error) {
        return <div>Failed to load albums.</div>;
    }

    function onSelect(albumId: string) {
        setSelectedAlbumId(albumId);
        setSelectedTrackId(undefined);
    }

    function onSelectTrack(trackId: string) {
        setSelectedTrackId(trackId);
    }

    return (
        <AppShell
            header={{height: 60}}
            padding="md"
        >
            <AppShell.Header>
                <Group h="100%" px="md" justify="space-between">
                    <Title order={3}>Music Catalog</Title>

                    <MetadataSearchBox onAlbumSelected={(releaseGroupId) => {
                        setSelectedReleaseGroupId(releaseGroupId);
                        setImportModalOpen(true);
                    }}/>

                    <Space w={100}/>
                </Group>
            </AppShell.Header>

            <AppShell.Main>

                <AddAlbumModel importModalOpen={importModalOpen} selectedReleaseGroupId={selectedReleaseGroupId}
                               setImportModalOpen={setImportModalOpen}/>

                <PanelGroup>
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
                </PanelGroup>
            </AppShell.Main>
        </AppShell>)
}

export default App
