import {Card, Center, Container, Image, Stack, Text, Title} from "@mantine/core";
import albumCover from '../assets/ImageCompressed.png';
import type {Album} from "../models/album.ts";

type AlbumDetailsProps = {
    albums: Album[];
    selectedAlbumId: number | undefined;
}

export function AlbumDetails({albums, selectedAlbumId} : AlbumDetailsProps)
{
    const album = albums.find(a => a.id === selectedAlbumId);

    if (album === undefined)
    {
        return <Container p={"lg"}>
            <Center>
                <Text>Please select an album to begin.</Text>
            </Center>
        </Container>
    }

    return <Container p="lg">
        <Card>
            <Center m="lg">
                <Stack align={"center"} gap={3}>
                    <Image
                        src={albumCover}
                        w={400}
                        h={400}
                        radius="sm"/>

                    <Title order={1}>{album.name}</Title>
                    <Text>{album.artist}</Text>
                </Stack>
            </Center>

            <Card bg={"dark"}>
                <Stack>
                    <Title order={2}>Details:</Title>
                    <Text>Release Year: {album.releaseYear}</Text>
                    <Text>Rating: {album.rating}</Text>
                </Stack>
            </Card>
        </Card>
    </Container>
}
