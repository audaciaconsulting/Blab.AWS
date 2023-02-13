export class AddPhoto {
  base64Photo!: string;
  name!: string;
  imageType!: string;
  size!: number;

  constructor(b64Photo: string, name: string, type: string, size: number) {
    this.base64Photo = b64Photo;
    this.name = name;
    this.imageType = type;
    this.size = size;
  }
}
