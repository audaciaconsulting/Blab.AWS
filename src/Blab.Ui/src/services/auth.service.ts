import { UserManager, WebStorageStateStore, User } from "oidc-client-ts";
import type { UserManagerSettings } from "oidc-client-ts";
import router from "@/router";

export class AuthService {
  private userManager: UserManager;

  constructor() {
    const settings: UserManagerSettings = {
      userStore: new WebStorageStateStore({ store: window.localStorage }),
      authority: import.meta.env.VITE_IDENTITY_URL,
      client_id: "blab-ui",
      redirect_uri: `${window.location.origin}/auth-callback`,
      automaticSilentRenew: true,
      silent_redirect_uri: `${window.location.origin}/silent-renew-callback`,
      response_type: "code",
      scope: "openid profile api",
      post_logout_redirect_uri: import.meta.env.VITE_UI_URL,
      filterProtocolClaims: true,
    };

    this.userManager = new UserManager(settings);
  }

  public getUser(): Promise<User | null> {
    return this.userManager.getUser();
  }

  public async login(returnUrl: string): Promise<void> {
    await this.userManager.signinRedirect({ state: returnUrl });
  }

  public async loginCallback(): Promise<void> {
    const user = await this.userManager.signinRedirectCallback();
    if (user) {
      router.push("/");
    }
  }

  public async logout(): Promise<void> {
    await this.userManager.signoutRedirect();
  }

  public async signInSilent(): Promise<void> {
    await this.userManager.signinSilent();
  }

  public async signInSilentCallback(): Promise<void> {
    await this.userManager.signinSilentCallback();
  }

  public async getAccessToken(): Promise<string | null> {
    let user = await this.getUser();

    if (user) {
      // If the user doesn't have a token or it has expired, sign them in again to get a new token
      if (!user.access_token || user.expired) {
        await this.signInSilent();
        user = await this.getUser();

        if (!user) {
          return null;
        }
      }

      return user.access_token;
    }

    return null;
  }
}

export const authService = new AuthService();
