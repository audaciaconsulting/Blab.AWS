import type { PhotoType } from "./photo-type.enum";

export class Photo {
  id!: number;
  blobName!: string;
  name!: string;
  type!: PhotoType;
}
