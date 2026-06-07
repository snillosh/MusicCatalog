import {Avatar, Card, Container, Group, NavLink, Stack, Text} from "@mantine/core";
import {type AlbumListItem} from "../api/albums.ts";

interface AlbumListProps {
    albums: AlbumListItem[];
    selectedId?: string;
    onSelect: (id: string) => void;
}

export function AlbumList({
                              albums,
                              selectedId,
                              onSelect
                          }: AlbumListProps) {

    return (
        <Container>
            <Card p="lg" bg="dark.8" mt="20" shadow="lg">
                <Stack gap={4}>
                    {albums.map((album) => (
                        <NavLink
                            key={album.id}
                            active={selectedId === album.id}
                            onClick={() => onSelect(album.id)}
                            label={
                                <Group wrap="nowrap">
                                    <Avatar
                                        src={`https://coverartarchive.org/release/${album.musicBrainzReleaseId}/front-250`}
                                        size={48}
                                        radius="sm"
                                    />
                                    <div>
                                        <Text>{album.title}</Text>
                                        <Text size="sm" c="dimmed">
                                            {album.artistName}
                                        </Text>
                                    </div>
                                </Group>
                            }
                        />
                    ))}
                </Stack>
            </Card>
        </Container>
    );
}
