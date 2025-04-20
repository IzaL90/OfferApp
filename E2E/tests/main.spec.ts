import { test, expect } from "@playwright/test";
import { SidebarPage } from "../pageObjects/SidebarPage";
import { TablePage } from "../pageObjects/TablePage";
import { SitePage } from "../pageObjects/SitePage";
import { EditComponent } from "../components/EditPage";
import { defaultBid, editBid } from "../interfaces/Bid";
import { PublishedBidsPage } from "../pageObjects/PublishedBidsPage";

test.beforeEach("Go to page", async ({ page }) => {
  await page.goto("http://localhost:5050");
});

test("Smoke tests", async ({ page }) => {
  await expect(
    page.locator("//table[@class='table table-striped']")
  ).toBeVisible();
  await expect(
    page.locator("//h1[@data-name='offers-information']")
  ).toHaveText("Hello, welcome to bid management!");
});

test("Sidebar test", async ({ page }) => {
  const sidebar = new SidebarPage(page);
  await sidebar.isVisible();
  await sidebar.clickHome();
  await expect(sidebar.clickedHome).toHaveClass(/active/);
  await sidebar.clickAperture();
});

test("Table visibility test", async ({ page }) => {
  const table = new TablePage(page);
  await table.isVisible();
  await table.expectColumns();
});

test("Add button test", async ({ page }) => {
  const site = new SitePage(page);
  await site.isVisible();
  await site.expectText();
  await site.clickButton();
  await site.modal.expectModalVisible();
});

test("Edit", async ({ page }) => {
  const table = new TablePage(page);
  await table.isVisible();
  await table.editButton.click();
  await table.edit.expectFormVisible();
});

test("Delete", async ({ page }) => {
  const table = new TablePage(page);
  await table.isVisible();
  await table.deleteButton.click();
  await table.delete.expectModalVisible();
});

test("View", async ({ page }) => {
  const table = new TablePage(page);
  await table.isVisible();
  await table.clickView();
});

test("Add BID", async ({ page }) => {
  const site = new SitePage(page);
  const table = new TablePage(page);
  await site.isVisible();
  await site.expectText();
  await site.clickButton();
  await site.modal.expectModalVisible();
  await site.modal.fillForm();
  await site.modal.clickSubmit();
  await expect(table.root).toContainText(defaultBid.name);
});

test("Edit BID", async ({ page }) => {
  const site = new SitePage(page);
  const table = new TablePage(page);
  await site.isVisible();
  await table.clickEdit(defaultBid.name);
  await table.edit.expectFormVisible();
  await table.edit.editForm();
  await table.edit.clickSubmit();
  await expect(table.root).toContainText(editBid.name);
});

test("Publish BID", async ({ page }) => {
  const site = new SitePage(page);
  const table = new TablePage(page);
  const sidebar = new SidebarPage(page);
  const publishedBidsPage = new PublishedBidsPage(page)
  await site.isVisible();
  await table.getRowLocator(editBid.name).click()
  expect(table.publishUnpublish.expectButtonVisible(table.publishUnpublish.publish))
  await table.publishUnpublish.publish.click({timeout:3000})
  await sidebar.clickAperture()
  expect(publishedBidsPage.getRowLocator(editBid.name)).toBeVisible()
});

test("Delete BID", async ({ page }) => {
  const site = new SitePage(page);
  const table = new TablePage(page);
  await table.clickDelete(editBid.name);
  await site.deleteModal.clickYes();
  await expect(table.row).toBeHidden();
});