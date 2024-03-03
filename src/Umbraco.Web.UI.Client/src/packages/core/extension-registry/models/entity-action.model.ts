import type { UmbControllerHostElement } from '@umbraco-cms/backoffice/controller-api';
import type { UmbEntityAction } from '@umbraco-cms/backoffice/entity-action';
import type { ManifestElementAndApi, ManifestWithDynamicConditions } from '@umbraco-cms/backoffice/extension-api';

/**
 * An action to perform on an entity
 * For example for content you may wish to create a new document etc
 */
// TODO: create interface for API
export interface ManifestEntityAction<MetaType extends MetaEntityAction>
	extends ManifestElementAndApi<UmbControllerHostElement, UmbEntityAction<MetaType>>,
		ManifestWithDynamicConditions {
	type: 'entityAction';
	meta: MetaType;
}

export interface MetaEntityAction {
	/**
	 * An icon to represent the action to be performed
	 *
	 * @examples [
	 *   "icon-box",
	 *   "icon-grid"
	 * ]
	 */
	icon?: string;

	/**
	 * The friendly name of the action to perform
	 *
	 * @examples [
	 *   "Create",
	 *   "Create Content Template"
	 * ]
	 */
	label?: string;

	entityTypes: Array<string>;
}

// DELETE
export interface ManifestEntityActionDeleteKind extends ManifestEntityAction<MetaEntityActionDeleteKind> {
	type: 'entityAction';
	kind: 'delete';
}

export interface MetaEntityActionDeleteKind extends MetaEntityAction {
	detailRepositoryAlias: string;
	itemRepositoryAlias: string;
}

// RENAME
export interface ManifestEntityActionRenameKind extends ManifestEntityAction<MetaEntityActionRenameKind> {
	type: 'entityAction';
	kind: 'rename';
}

export interface MetaEntityActionRenameKind extends MetaEntityAction {
	renameRepositoryAlias: string;
	itemRepositoryAlias: string;
}

// RELOAD TREE ITEM CHILDREN
export interface ManifestEntityActionReloadTreeItemChildrenKind
	extends ManifestEntityAction<MetaEntityActionRenameKind> {
	type: 'entityAction';
	kind: 'reloadTreeItemChildren';
}

export interface MetaEntityActionReloadTreeItemChildrenKind extends MetaEntityAction {}

// DUPLICATE
export interface ManifestEntityActionDuplicateKind extends ManifestEntityAction<MetaEntityActionDuplicateKind> {
	type: 'entityAction';
	kind: 'duplicate';
}

export interface MetaEntityActionDuplicateKind extends MetaEntityAction {
	duplicateRepositoryAlias: string;
	itemRepositoryAlias: string;
	pickerModalAlias: string;
}

// MOVE

export interface ManifestEntityActionMoveKind extends ManifestEntityAction<MetaEntityActionMoveKind> {
	type: 'entityAction';
	kind: 'move';
}

export interface MetaEntityActionMoveKind extends MetaEntityAction {
	moveRepositoryAlias: string;
	itemRepositoryAlias: string;
	pickerModalAlias: string;
}
