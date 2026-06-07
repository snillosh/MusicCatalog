import {Card, Center, Container, Image, Stack, Text, Title} from "@mantine/core";
import type {AlbumListItem} from "../api/albums.ts";

type AlbumDetailsProps = {
    albums: AlbumListItem[];
    selectedAlbumId: string | undefined;
}

export function AlbumDetails({albums, selectedAlbumId}: AlbumDetailsProps) {
    const album = albums.find(a => a.id === selectedAlbumId);

    if (album === undefined) {
        return <Container p={"lg"}>
            <Center>
                <Text>Please select an album to begin.</Text>
            </Center>
        </Container>
    }

    return <Container p="lg">
        <Card shadow="lg">
            <Center m="lg">
                <Stack align={"center"} gap={3}>
                    <Image
                        src={`https://coverartarchive.org/release/${album.musicBrainzReleaseId}/front-250`}
                        w={400}
                        h={400}
                        radius="sm"/>

                    <Title order={1}>{album.title}</Title>
                    <Text>{album.artistName}</Text>
                </Stack>
            </Center>

            <Card bg={"dark"}>
                <Stack>
                    <Title order={2}>Details:</Title>
                    <Text>Release Year: {album.releaseYear}</Text>
                    <Text>Rating: {album.releaseYear}</Text>
                </Stack>
            </Card>
        </Card>
    </Container>
}
