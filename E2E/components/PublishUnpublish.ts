import { expect, Page } from "@playwright/test";
import { Locator } from "playwright";

export class PublishUnpublishComponent {
  public readonly root: Locator;
  public readonly publish: Locator;
  public readonly unpublish: Locator;

  constructor(root: Locator) {
    this.root = root;
    this.publish = this.root.locator("//*[@data-name='bid-set-as-publish-button']");
    this.unpublish = this.root.locator(
      "//*[@data-name='bid-set-as-unpublish-button']"
    );
  }

  public async expectButtonVisible(element:Locator): Promise<void>{
    await expect(element).toBeVisible()
  }
}
