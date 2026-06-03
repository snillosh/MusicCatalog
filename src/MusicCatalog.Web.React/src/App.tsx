import './App.css'
import {useState} from "react";
import type {Album} from "./models/album.ts";
import {AlbumList} from "./components/AlbumList.tsx";
import {Box, Card} from "@mantine/core";
import {Panel, Group, Separator} from "react-resizable-panels";

function App() {

    const [albums] = useState<Album[]>([{id: 0, name: "Imaginal Disk", artist: "Magdalena Bay"}, {id: 0, name: "Entertainment!", artist: "Gang of Four"}]);

    function onSelect()
    {

    }



  return (<Group>
      <Panel defaultSize={300} minSize={200}>
          <Card bg="dark">
              <AlbumList albums={albums} onSelect={onSelect}/>
          </Card>
      </Panel>

      <Separator />


      <Panel minSize={500}>
          <Box p="md">Album details</Box>
      </Panel>

      <Separator />

      <Panel defaultSize={300} minSize={200}>
          <Card bg="dark">
              <Box p="md">External Links</Box>
          </Card>
      </Panel>
  </Group>)
}

export default App
