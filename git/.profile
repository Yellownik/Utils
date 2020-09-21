### git add
alias ga='git number add'
alias gaa='git add .'
alias gau='git add -u'
alias gaaa='git add --all'
alias gaou='git add-ours'
alias gath='git add-their'
### git branch
alias gb='git branch'
alias gba='git branch --all'
alias gbag='git branch-grep'
alias gbc='git branch --contains'
alias gbd='git branch -D'
alias gbt='git branch-template'
alias gbmh='git branch -m $(git branch-name)'
### git commit
alias gc='git commit'
alias gcm='git commit -m'
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
alias gob='git checkout -b'
alias god='git checkout develop'
alias gom='git checkout master'
alias got='git checkout --track'
alias goto='git got-origin'
### fetch/pull/push
alias gfo='git fetch origin'
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
###
alias gu='git number reset HEAD'
alias wtf='git reset --hard head'
alias smc='git my-commits'

function _branch_publish(){
    if ! [ -z $1 ]; then
        git push origin "$1":"$1"
    else
	    git push origin $(git branch-name)
  fi
}
alias gph='_branch_publish'

function _branch_update(){
    if ! [ -z $1 ]; then
        git fetch origin "$1":"$1"
    else
         git pull origin $(git branch-name)
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
