### git add
alias ga='git number add'
alias gaa='git add .'
alias gac='git add *.cs'
alias gapc='git add -p *.cs'
alias gau='git add -u'
alias gaaa='git add --all'
alias gaou='git add-ours'
alias gath='git add-their'

### git branch
alias gb='git branch'
alias gba='git branch --all'
function _branch_grep(){
    git branch --all | grep "$1" | while read -r line; do echo "${line/remotes\/}"; done
}
alias gbag='_branch_grep'
alias gbaf='_branch_grep'

function _get_build_version(){
    git fetch --prune origin
    build=$(git branch --remote | grep origin/build/v | sort -V --version-sort | tail -1)
    v_build="${build/origin\/build\/}"
    echo $v_build
}
function _branch_version(){ 
    git checkout -b "$(_get_build_version)"/"$1"
}
alias gbv='_branch_version'

function _branch_build(){ 
    git checkout -b "$(_get_build_version)"/build/"$1"
}
alias gbb='_branch_build'

function _branch_feature(){ 
    git checkout -b "$(git config --global user.feature)"/"$1"
}
alias gbf='_branch_feature'

function _go_build(){
    build=$(git branch | grep build/v | sort -V --version-sort | tail -1 | xargs)
    echo "$build"
    git checkout "$build"
}
alias gob='_go_build'

alias gbc='git branch --contains'
alias gbd='git branch -D'
alias gbmh='git branch -m $(git branch-name)'

### git commit
alias gc='git commit'

function _add_task_code_to_commit(){
    code=$( _branch_name | egrep -o 'MP-[[:digit:]]*')
    message="$code $1"
	git commit -m "$code $1"
}
alias gcm='_add_task_code_to_commit'

alias gca='git commit --amend'
alias gcano='git commit --amend --no-edit'
alias gcp='git cherry-pick'

### git diff
alias gd='git number diff'
alias gdc='git number diff --cached'
alias gdu='git diff --name-only --diff-filter=U'
alias gds='git diff --stat'
alias gdh='git diff HEAD~1 HEAD'
alias gdhs='git diff --stat HEAD~1 HEAD'

### git history/status
alias gk='gitk --all'
alias gh='git history --all'
alias ghh='git history'
alias gs='git number -s'
alias gsg='git status-grep'

### git merge
alias gm='git merge'
alias gma='git merge --abort'
alias gmd='git merge develop'
alias gmnf='git merge --no-ff'

### git checkout
alias go='git number checkout'
alias got='git checkout --track'
alias god='git checkout develop'
alias gom='git checkout master'
alias goto='git got-origin'

### fetch/pull/push
alias gfo='git fetch --prune origin'

### git rebase
alias gr='git rebase'
alias gra='git rebase --abort'
alias gri='git rebase -i'
alias grih='git rih'
alias grc='git rebase --continue'
alias grod='git rebase --onto develop develop $(git branch-name)'
alias gropr='git rebase --onto $(git prev-branch) $(git prev-branch) $(git branch-name)'

### git stash
alias gst='git number stash'
alias gsta='git stash apply'
alias gstc='git stash clear'
alias gstp='git stash pop'
alias gsts='git stash show'
alias gstst='git stash -- $(git diff --staged --name-only)'
###
alias gu='git number reset HEAD'
alias smc='git my-commits'

function _reset_all(){
    git add --all
    git reset --hard head
}
alias wtf='_reset_all'

function _show_merged(){
    git branch --merged| grep -v -E 'master|"$(_get_build_version)"'
}
alias gsm='_show_merged'

function _remove_merged(){
    git branch --merged| grep -v -E 'master|"$(_get_build_version)"' | xargs git branch -d
}
alias grm='_remove_merged'

function _branch_name(){
    git rev-parse --abbrev-ref HEAD
}

function _branch_publish(){
    if ! [ -z $1 ]; then
        git push origin "$1":"$1"
    else
        git push origin $(_branch_name)
  fi
}
alias gph='_branch_publish'

function _branch_update(){
    if ! [ -z $1 ]; then
        git fetch origin "$1":"$1"
    else
         git pull origin $(_branch_name)
  fi
}
alias gpl="_branch_update"
alias gpld='_branch_update develop'

### pull "main_branch", create "new_branch" from "main_branch", push "new_branch" and remove it locally
### $ _update_branch_and_create_new develop team_1/build/my_test
function _update_branch_and_create_new(){
    main_branch=$1
    if [ -z $main_branch ]; then
        echo "Please specify a branch to update."
        return
    fi

    new_branch=$2
    if [ -z $new_branch ]; then
        echo "Please specify a new branch name."
        return
    fi

    git fetch origin "$main_branch":"$main_branch"
    git branch "$new_branch" "$main_branch"
    git push origin "$new_branch":"$new_branch"

    git show --oneline "$new_branch"
    git branch -D "$new_branch"
}

function _go_to_branch() {
    local is_local=$(git branch --list "$1")
    if [[ "$1" == "-" ]]; then
        git checkout -
        return
    fi

    local is_local=$(git branch --list "$1")
    if [[ -n ${is_local} ]]; then
        git checkout $1
        return
    fi

    local no_origin_local="${1/origin\/}"
    local is_no_origine_local=$(git branch --list "$no_origin_local")
    if [[ -n ${is_no_origine_local} ]]; then
        git checkout $no_origin_local
        return
    fi

    local is_remote=$(git branch --remote --list "$1")
    if [[ -n ${is_remote} ]]; then
        git checkout --track $1
        return
    fi

    echo branch \'${1}\' does not exist
}
alias goto="_go_to_branch"