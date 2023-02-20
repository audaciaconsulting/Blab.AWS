import type { IEnvironmentConfig } from "@/models/config/environment-config.interface";

export let config = {} as IEnvironmentConfig;

export class ConfigService {
  private configUrl = "assets/config.json";

  async load(): Promise<void> {
    const baseUrl = import.meta.env.BASE_URL;
    const configResponse = await fetch(baseUrl + this.configUrl);
    const configData: IEnvironmentConfig = await configResponse.json();

    if (configData) {
      config = configData;
    }
  }
}
