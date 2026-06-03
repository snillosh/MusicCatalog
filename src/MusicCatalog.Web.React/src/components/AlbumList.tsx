import type {Album} from "../models/album.ts";
import {Avatar, Group, Stack, Text, NavLink, Card, Container} from "@mantine/core";
import albumCover from '../assets/ImageCompressed.png';

interface AlbumListProps {
    albums: Album[];
    selectedId?: number;
    onSelect: (id: number) => void;
}

export function AlbumList({
                              albums,
                              selectedId,
                              onSelect,
                          }: AlbumListProps) {


    return (
        <Container>
            <Card p="lg" bg="dark.8" mt="20">
                <Stack gap={4}>
                    {albums.map((album) => (
                        <NavLink
                            active={selectedId === album.id}
                            onClick={() => onSelect(album.id)}
                            label={
                                <Group wrap="nowrap">
                                    <Avatar
                                        src={albumCover}
                                        size={48}
                                        radius="sm"
                                    />
                                    <div>
                                        <Text>{album.name}</Text>
                                        <Text size="sm" c="dimmed">
                                            {album.artist}
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
