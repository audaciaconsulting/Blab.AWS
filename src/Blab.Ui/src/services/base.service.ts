import { ApiResponse } from "@/models/api/api-response.model";
import { authService } from "@/services/auth.service";

export class BaseService {
  private url: string;

  /**
   * Base service for calling HTTP requests using fetch.
   */
  constructor(baseUrl: string | null = null) {
    if (baseUrl === null || baseUrl === undefined) {
      this.url = import.meta.env.VITE_API_URL as string;
    } else {
      this.url = baseUrl;
    }
  }

  init(): Promise<void> {
    return new Promise((resolve) => {
      document.addEventListener("readystatechange", () => {
        if (document.readyState === "complete") {
          resolve();
        }
      });
    });
  }
  private async getHeaders(isFormData = false): Promise<Headers> {
    const headers = new Headers();
    if (!isFormData) {
      headers.append("Content-Type", "application/json");

      const token = await authService.getAccessToken();
      if (token) {
        headers.append("Authorization", `Bearer ${token}`);
      }
    }
    return headers;
  }
  public async get(endpoint: string): Promise<any> {
    const headers = await this.getHeaders();
    if (!headers) {
      return Promise.reject();
    }
    const info = {
      method: "GET",
      headers: headers,
    };
    return new Promise((resolve, reject) => {
      fetch(this.url + endpoint, info).then((res) =>
        this.parseJsonResponse(res, resolve, reject)
      );
    });
  }
  public async post(
    endpoint: string,
    payload: Record<string, any> | FormData
  ): Promise<any> {
    const isFormData = payload instanceof FormData;
    const headers = await this.getHeaders(isFormData);
    return new Promise((resolve, reject) => {
      fetch(this.url + endpoint, {
        method: "POST",
        headers: headers,
        body: isFormData ? payload : JSON.stringify(payload),
      }).then((res) => this.parseJsonResponse(res, resolve, reject));
    });
  }
  public async put(
    endpoint: string,
    payload: Record<string, any> | FormData
  ): Promise<any> {
    const isFormData = payload instanceof FormData;
    const headers = await this.getHeaders(isFormData);
    return new Promise((resolve, reject) => {
      fetch(this.url + endpoint, {
        method: "PUT",
        headers: headers,
        body: isFormData ? payload : JSON.stringify(payload),
      }).then((res) => {
        this.parseJsonResponse(res, resolve, reject);
      });
    });
  }
  public async delete(endpoint: string): Promise<any> {
    const headers = await this.getHeaders();
    return new Promise((resolve, reject) => {
      fetch(this.url + endpoint, {
        method: "DELETE",
        headers: headers,
      }).then((res) => this.parseJsonResponse(res, resolve, reject));
    });
  }
  public async getBlob(endpoint: string): Promise<Blob> {
    const headers = await this.getHeaders();
    return new Promise((resolve, reject) => {
      fetch(this.url + endpoint, {
        method: "GET",
        headers: headers,
      })
        .then((res) => res.blob())
        .then((blob) => {
          resolve(blob);
        })
        .catch((err) => {
          reject(err);
        });
    });
  }
  public parseJsonResponse(
    res: Response,
    resolve: (value: unknown) => void,
    reject: (value: unknown) => void
  ) {
    const apiResponse: ApiResponse = new ApiResponse(res.status);
    res
      .text()
      .then((text) => {
        if (text) {
          try {
            const data = JSON.parse(text);
            apiResponse.data = data;

            resolve(apiResponse);
          } catch (text) {
            apiResponse.data = text;

            resolve(apiResponse);
          }
        } else {
          apiResponse.data = text;

          resolve(apiResponse);
        }
      })
      .catch((err) => {
        apiResponse.data = err;
        reject(apiResponse);
      });
  }
}

const baseService = new BaseService();
export default baseService;
